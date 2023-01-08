import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { WebGLService } from 'src/app/services/webgl.service';
import vertexSrc from '../../../assets/shaders/vertex.glsl';
import fragmentSrc from '../../../assets/shaders/fragment.glsl';
import standVertexSrc from '../../../assets/shaders/stand_vertex.glsl';
import standFragmentSrc from '../../../assets/shaders/stand_fragment.glsl';
import { MovieCover } from './moviecover';
import { Camera } from './camera';
import { mat4 } from 'gl-matrix';
import { Stand } from './stand';
import { FavoriteMoviesService } from 'src/app/services/favoritemovies.service';
import { UserService } from 'src/app/services/user.service';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-favorites-graphics',
  templateUrl: './favorites-graphics.component.html',
  styleUrls: ['./favorites-graphics.component.css']
})
export class FavoritesGraphicsComponent implements OnInit, AfterViewInit {

  @ViewChild("canvas") private canvas! : ElementRef<HTMLCanvasElement>;

  private defaultShader! : WebGLProgram;
  private standShader! : WebGLProgram;

  private movieCovers! : MovieCover[];
  private stand! : Stand;
  private coverImages! : HTMLImageElement[];
  private camera! : Camera;

  private aspectRatio! : number;

  private lastTime! : number;
  private deltaTime! : number;
  private allowedTraversal! : number;

  private shouldLoad! : boolean;

  constructor(private webglService : WebGLService, private favoriteMoviesService : FavoriteMoviesService, private userService : UserService) { }

  ngOnInit(): void {

    this.movieCovers = new Array();
    this.coverImages = new Array();
    this.camera = new Camera();
    this.lastTime = 0.0;
    this.deltaTime = 0.0;
    this.allowedTraversal = 0.0;
    this.shouldLoad = true;
  }

  async ngAfterViewInit(): Promise<void> {

    if (!this.webglService.initWebGL(this.canvas.nativeElement)) {
      return;
    }

    await this.loadImages();

    if (!this.shouldLoad) {
      return;
    }

    this.webglService.setViewport(this.canvas.nativeElement.width, this.canvas.nativeElement.height);
    this.aspectRatio = this.canvas.nativeElement.width / this.canvas.nativeElement.height;
    this.camera.updatePerspective(45.0, this.aspectRatio, 0.1, 1000.0);

    let shaderProgram = this.webglService.createShaderProgram(vertexSrc, fragmentSrc);

    if (!shaderProgram) {
      return;
    }

    this.defaultShader = shaderProgram;

    shaderProgram = this.webglService.createShaderProgram(standVertexSrc, standFragmentSrc);

    if (!shaderProgram) {
      return;
    }

    this.standShader = shaderProgram;

    this.webglService.bindShader(this.defaultShader);
    this.loadStand();
    
    this.canvas.nativeElement.addEventListener("mousedown", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })
    this.canvas.nativeElement.addEventListener("mouseup", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })
    this.canvas.nativeElement.addEventListener("mousemove", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })
    this.canvas.nativeElement.addEventListener("mouseleave", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })

    window.requestAnimationFrame((timestamp : number) => {
      this.draw(timestamp);
    });
  }

  draw(timestamp : number) : void {
    
    this.webglService.clear();

    this.webglService.bindShader(this.defaultShader);
    this.webglService.setUniformMatrix(this.defaultShader, "uProj", this.camera.getProjMatrix());
    this.webglService.setUniformMatrix(this.defaultShader, "uView", this.camera.getViewMatrix());
    this.webglService.bindShader(this.standShader);
    this.webglService.setUniformMatrix(this.standShader, "uProj", this.camera.getProjMatrix());
    this.webglService.setUniformMatrix(this.standShader, "uView", this.camera.getViewMatrix());

    this.webglService.bindShader(this.defaultShader);

    this.movieCovers.forEach(movieCover => {
        
      this.webglService.bindVAO(movieCover.getFrontBackVAO());
      this.webglService.bindTexture(movieCover.getMainTex());
      this.webglService.setUniformMatrix(this.defaultShader, "uModel", movieCover.getModelMatrix());

      this.webglService.drawArrays(0, movieCover.getFrontBackVertCount());

      this.webglService.bindVAO(movieCover.getSideVAO());
      this.webglService.bindTexture(movieCover.getSideTex());

      this.webglService.drawArrays(0, movieCover.getSideVertCount() - 1);
    });

    this.webglService.bindShader(this.standShader);

    this.webglService.bindVAO(this.stand.getVAO());
    this.webglService.setUniformMatrix(this.standShader, "uModel", this.stand.getModelMatrix());
    this.webglService.drawArrays(0, this.stand.getVertCount()); 

    this.onResize();
    this.camera.updateView();

    this.deltaTime = (timestamp - this.lastTime) / 1000.0;
    this.lastTime = timestamp;

    window.requestAnimationFrame((timestamp : number) => {
      this.draw(timestamp);
    });
  }

  private onResize() : void {
    const displayWidth = this.canvas.nativeElement.clientWidth;
    const displayHeight = this.canvas.nativeElement.clientHeight;

    const needResize = this.canvas.nativeElement.width !== displayWidth || this.canvas.nativeElement.height !== displayHeight;

    if (needResize) {
      this.canvas.nativeElement.width = displayWidth;
      this.canvas.nativeElement.height = displayHeight;

      this.webglService.setViewport(this.canvas.nativeElement.width, this.canvas.nativeElement.height);
      this.aspectRatio = this.canvas.nativeElement.width / this.canvas.nativeElement.height;
      this.camera.updatePerspective(45.0, this.aspectRatio, 0.1, 1000.0);
    }
  }

  private loadStand() : void {
    this.stand = new Stand(this.webglService, this.movieCovers.length * 2.5);
    this.stand.setPosition([-3.0, -1.0, -6.0]);
    this.stand.setRotation([0, 0, 1], -90 * (Math.PI / 180.0));
    this.stand.setRotation([0, 1, 0], -45 * (Math.PI / 180.0));
  }

  private async loadImages() : Promise<void> {

    var currentUser = await this.userService.getCurrentUser();
    const favoriteMovies = await firstValueFrom(this.favoriteMoviesService.getFavoriteMovies(currentUser!.username));

    if (!favoriteMovies.ok) {
      this.shouldLoad = false;
      return;
    }

    for (var i = 0; i < favoriteMovies.body!.length; i++) {
      var image = new Image();
      image.src = favoriteMovies.body![i].coverArt;
      image.crossOrigin = "anonymous";
      this.coverImages.push(image);
    }

    var lastPos = -2.5;
    for (var i = 0; i < this.coverImages.length; i++) {
      this.movieCovers.push(new MovieCover(this.webglService));
      this.movieCovers[i].setPosition([lastPos, 0.0, -5.0]);
      lastPos += 2.5;
    }

    this.allowedTraversal = lastPos;

    let copy = (i : number) : void => {
      this.coverImages[i].addEventListener("load", () => {
        this.webglService.copyToTexture(this.movieCovers[i].getMainTex(), this.coverImages[i]);
      });
    };

    for (var i = 0; i < this.movieCovers.length; i++) {
      copy(i);
    }
  }
}
