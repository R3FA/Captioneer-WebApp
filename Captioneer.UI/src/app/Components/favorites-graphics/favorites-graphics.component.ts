import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { WebGLService } from 'src/app/services/webgl.service';
import vertexSrc from '../../../assets/shaders/vertex.glsl';
import fragmentSrc from '../../../assets/shaders/fragment.glsl';
import standVertexSrc from '../../../assets/shaders/stand_vertex.glsl';
import standFragmentSrc from '../../../assets/shaders/stand_fragment.glsl';
import { Cover } from './cover';
import { Camera } from './camera';
import { Stand } from './stand';
import { FavoriteMoviesService } from 'src/app/services/favoritemovies.service';
import { UserService } from 'src/app/services/user.service';
import { firstValueFrom } from 'rxjs';
import { mat4, vec2, vec3 } from 'gl-matrix';
import { FavoriteTVShowsService } from 'src/app/services/favoritetvshows.service';
import { MovieViewModel } from 'src/app/models/movie-viewmodel';
import { TVShowViewModel } from 'src/app/models/tvshow-viewmodel';

@Component({
  selector: 'app-favorites-graphics',
  templateUrl: './favorites-graphics.component.html',
  styleUrls: ['./favorites-graphics.component.css']
})
export class FavoritesGraphicsComponent implements OnInit, AfterViewInit {

  @ViewChild("canvas") private canvas! : ElementRef<HTMLCanvasElement>;

  private defaultShader! : WebGLProgram;
  private standShader! : WebGLProgram;

  private covers! : Cover[];
  private stand! : Stand;
  private coverImages! : HTMLImageElement[];
  private camera! : Camera;

  private aspectRatio! : number;

  private lastTime! : number;
  private deltaTime! : number;
  private allowedTraversal! : number;
  private mousePosition! : vec2;
  private canvasRectTopLeft! : vec2;
  private canvasRectDimensions! : vec2;
  private lightPosition! : vec3;

  private shouldLoad! : boolean;

  constructor(private webglService : WebGLService, private favoriteMoviesService : FavoriteMoviesService, private favoriteTVShowsService : FavoriteTVShowsService, private userService : UserService) { }

  ngOnInit(): void {

    this.covers = new Array();
    this.coverImages = new Array();
    this.camera = new Camera();
    this.lastTime = 0.0;
    this.deltaTime = 0.0;
    this.allowedTraversal = 0.0;
    this.mousePosition = vec2.create();
    this.canvasRectTopLeft = vec2.create();
    this.canvasRectDimensions = vec2.create();
    this.lightPosition = vec3.create();
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
    this.camera.onResize(45.0, this.aspectRatio, 0.1, 1000.0);

    let shaderProgram = this.webglService.createShaderProgram(vertexSrc, fragmentSrc);

    if (!shaderProgram) {
      return;
    }

    this.defaultShader = shaderProgram;

    this.webglService.bindShader(this.defaultShader);
    this.webglService.setUniformVec3(this.defaultShader, "uMaterial.specular", [0.7, 0.7, 0.7]);
    this.webglService.setUniformFloat(this.defaultShader, "uMaterial.shininess", 32.0);
    this.webglService.bindShader(null);

    shaderProgram = this.webglService.createShaderProgram(standVertexSrc, standFragmentSrc);

    if (!shaderProgram) {
      return;
    }

    this.standShader = shaderProgram;

    this.webglService.bindShader(this.defaultShader);
    this.loadStand();
    
    this.canvasRectTopLeft = [this.canvas.nativeElement.getBoundingClientRect().left, this.canvas.nativeElement.getBoundingClientRect().top];
    this.canvasRectDimensions = [this.canvas.nativeElement.getBoundingClientRect().width, this.canvas.nativeElement.getBoundingClientRect().height];

    this.canvas.nativeElement.addEventListener("mousedown", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })
    this.canvas.nativeElement.addEventListener("mouseup", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
    })
    this.canvas.nativeElement.addEventListener("mousemove", (e) => {
      this.camera.onMouseEvent(e, this.deltaTime, this.allowedTraversal);
      this.calculateMousePosition(e);
      this.calculateLightPosition();
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
    this.webglService.setUniformVec2(this.defaultShader, "uLightPos", [this.lightPosition[0], this.lightPosition[1]]);
    this.webglService.bindShader(this.standShader);
    this.webglService.setUniformMatrix(this.standShader, "uProj", this.camera.getProjMatrix());
    this.webglService.setUniformMatrix(this.standShader, "uView", this.camera.getViewMatrix());
    this.webglService.setUniformVec2(this.standShader, "uLightPos", [this.lightPosition[0], this.lightPosition[1]]);

    this.webglService.bindShader(this.defaultShader);

    this.covers.forEach(movieCover => {
        
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
    this.camera.onUpdate();

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
      this.camera.onResize(45.0, this.aspectRatio, 0.1, 1000.0);

      this.canvasRectTopLeft = [this.canvas.nativeElement.getBoundingClientRect().left, this.canvas.nativeElement.getBoundingClientRect().top];
      this.canvasRectDimensions = [this.canvas.nativeElement.getBoundingClientRect().width, this.canvas.nativeElement.getBoundingClientRect().height];
    }
  }

  private loadStand() : void {
    this.stand = new Stand(this.webglService, this.covers.length * 2.5);
    this.stand.setPosition([-3.0, -1.0, -6.0]);
    this.stand.setRotation([0, 0, 1], -90 * (Math.PI / 180.0));
    this.stand.setRotation([0, 1, 0], -45 * (Math.PI / 180.0));
  }

  private async loadImages() : Promise<void> {

    var currentUser = await this.userService.getCurrentUser();

    if (!currentUser) {
      return;
    }
    
    var favoriteMovies = await this.favoriteMoviesService.getFavoriteMovies(currentUser.username);
    var favoriteShows = await this.favoriteTVShowsService.getFavoriteShows(currentUser.username);
  
    if (favoriteMovies) {
      for (var i = 0; i < favoriteMovies.length; i++) {
        var image = new Image();
        image.src = favoriteMovies[i].coverArt;
        image.crossOrigin = "anonymous";
        this.coverImages.push(image);
      }
    }

    if (favoriteShows) {
      for (var i = 0; i < favoriteShows.length; i++) {
        var image = new Image();
        image.src = favoriteShows[i].coverArt;
        image.crossOrigin = "anonymous";
        this.coverImages.push(image);
      }
    }

    var lastPos = -2.5;
    for (var i = 0; i < this.coverImages.length; i++) {
      this.covers.push(new Cover(this.webglService));
      this.covers[i].setPosition([lastPos, 0.0, -5.0]);
      lastPos += 2.5;
    }

    this.allowedTraversal = lastPos - 2.5;

    let copy = (i : number) : void => {
      this.coverImages[i].addEventListener("load", () => {
        this.webglService.copyToTexture(this.covers[i].getMainTex(), this.coverImages[i]);
      });
    };

    for (var i = 0; i < this.covers.length; i++) {
      copy(i);
    }
  }

  private calculateMousePosition(event : MouseEvent)  {

    this.mousePosition = [event.clientX - this.canvasRectTopLeft[0], event.clientY - this.canvasRectTopLeft[1]];
  }

  private calculateLightPosition() : void {
    
    const clipX = this.mousePosition[0] / this.canvasRectDimensions[0] * 2 - 1;
    const clipY = this.mousePosition[1] / this.canvasRectDimensions[1] * -2 + 1;

    console.log(clipX);

    vec3.transformMat4(this.lightPosition, [clipX, clipY, -1], this.camera.getInverseViewProjMatrix());
  }
}
