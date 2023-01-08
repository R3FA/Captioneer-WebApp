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

@Component({
  selector: 'app-favorite-movies',
  templateUrl: './favorite-movies.component.html',
  styleUrls: ['./favorite-movies.component.css']
})
export class FavoriteMoviesComponent implements OnInit, AfterViewInit {

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

  // Dummy cover image
  private image1 = "../../assets/Pictures/heat-dummy.jpg";
  //

  constructor(private webglService : WebGLService) { }

  ngOnInit(): void {

    this.movieCovers = new Array();
    this.coverImages = new Array();
    this.camera = new Camera();
    this.lastTime = 0.0;
    this.deltaTime = 0.0;
    this.allowedTraversal = 0.0;
  }

  ngAfterViewInit(): void {
    if (!this.webglService.initWebGL(this.canvas.nativeElement)) {
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

    this.coverImages.push(new Image());
    
    this.loadMovieCovers();
    this.loadStand();
    this.loadImages();
    
    this.coverImages[0].crossOrigin = "anonymous";
    this.coverImages[0].src = this.image1;

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

  private loadMovieCovers() : void {

    var lastPos = -2.5;
    this.movieCovers.push(new MovieCover(this.webglService));
    this.movieCovers[0].setPosition([-2.5, 0.0, -5.0]);

    for (var i = 1; i < 10; i++) {
      this.movieCovers.push(new MovieCover(this.webglService));
      this.movieCovers[i].setPosition([lastPos + 2.5, 0.0, -5.0]);
      lastPos += 2.5;
    }

    this.allowedTraversal = lastPos;
  }

  private loadStand() : void {
    this.stand = new Stand(this.webglService, this.movieCovers.length * 2.5);
    this.stand.setPosition([-3.0, -1.0, -6.0]);
    this.stand.setRotation([0, 0, 1], -90 * (Math.PI / 180.0));
    this.stand.setRotation([0, 1, 0], -45 * (Math.PI / 180.0));
  }

  private loadImages() : void {

    let copy = (i : number) : void => {
      this.coverImages[0].addEventListener("load", () => {
        this.webglService.copyToTexture(this.movieCovers[i].getMainTex(), this.coverImages[0]);
      });
    };

    for (var i = 0; i < this.movieCovers.length; i++) {
      copy(i);
    }
  }
}
