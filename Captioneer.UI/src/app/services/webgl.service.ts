import { Injectable } from '@angular/core';
import { mat4 } from 'gl-matrix';
import { MovieCover } from '../Components/favorites-graphics/moviecover';

export enum DataType {
  GL_FLOAT = 1,
  GL_BYTE,
}

@Injectable({
  providedIn: 'root'
})
export class WebGLService {

  private gl! : WebGL2RenderingContext;
  private uniformLocations! : Map<WebGLProgram, Map<string, WebGLUniformLocation>>

  constructor() { }

  initWebGL(canvas : HTMLCanvasElement) : boolean {
    var context = canvas.getContext("webgl2");

    if (!context) {
      console.error("Unable to create context. Your browser may not support WebGL2.");
      return false;
    }

    this.gl = context;
    this.uniformLocations = new Map();

    this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, true);
    this.gl.enable(this.gl.DEPTH_TEST);
    this.gl.depthFunc(this.gl.LESS);
    this.gl.clearColor(0, 0, 0, 1);

    return true;
  }

  clear() : void {
    this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);
  }

  drawArrays(offset : number, length : number) : void {
    this.gl.drawArrays(this.gl.TRIANGLES, offset, length);
  }

  setUniformMatrix(shader : WebGLProgram, uniform : string, matrix : mat4) : void {

    this.cacheUniform(shader, uniform);
    this.gl.uniformMatrix4fv(<WebGLUniformLocation>this.uniformLocations.get(shader)?.get(uniform), false, matrix);
  }

  setUniformInteger(shader : WebGLProgram, uniform : string, value : number) : void {

    this.cacheUniform(shader, uniform);
    this.gl.uniform1iv(<WebGLUniformLocation>this.uniformLocations.get(shader)?.get(uniform), [value]);
  }

  private cacheUniform(shader : WebGLProgram, uniform : string) : void {

    var cachedShader = this.uniformLocations.get(shader);

    if (cachedShader == undefined)  {
      let location = this.gl.getUniformLocation(shader, uniform);

      if (!location) {
        console.error("The provided uniform was not found on the shader!");
        return;
      }

      this.uniformLocations.set(shader, new Map(
        [
          [uniform, location]
        ]
      ));
    }
    else {

      if (cachedShader.get(uniform) != undefined) {
        return;
      }

      let location = this.gl.getUniformLocation(shader, uniform);

      if (!location) {
        console.error("The provided uniform was not found on the shader!");
        return;
      }

      cachedShader.set(uniform, location);
    }
  }

  createBuffer(dataType : DataType, data : number[]) : WebGLBuffer | null { 
    
    var vbo = this.gl.createBuffer();
    var dataBuffer : any;

    if (!vbo) {
      console.error("Could not create VBO!");
      return null;
    }

    switch (dataType)
    {
      case DataType.GL_FLOAT: dataBuffer = new Float32Array(data);
      break;
      case DataType.GL_BYTE: dataBuffer = new Int8Array(data);
      break;
    };

    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vbo);
    this.gl.bufferData(this.gl.ARRAY_BUFFER, dataBuffer, this.gl.STATIC_DRAW);

    return vbo;
  }

  createVAO() : WebGLVertexArrayObject | null {

    var vao = this.gl.createVertexArray();

    if (!vao) {
      console.error("Could not create VAO!");
      return null;
    }

    return vao;
  }

  bindVAO(vao : WebGLVertexArrayObject | null) : void {
    this.gl.bindVertexArray(vao);
  }

  addBuffer(vao : WebGLVertexArrayObject, vbo : WebGLBuffer, location : number, componentCount : number, dataType : DataType) : void {

    this.gl.bindVertexArray(vao);
    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vbo);

    this.gl.enableVertexAttribArray(location);
    if (dataType == DataType.GL_FLOAT)  {
      this.gl.vertexAttribPointer(location, componentCount, this.getGLType(dataType), false, 0, 0);
    }
    else {
      this.gl.vertexAttribIPointer(location, componentCount, this.getGLType(dataType), 0, 0);
    }

    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
    this.gl.bindVertexArray(null);
  }

  createShaderProgram(vertexSrc : string, fragmentSrc : string) : WebGLProgram | null {
    
    var vertexShader = this.compileShader(this.gl.VERTEX_SHADER, vertexSrc);
    var fragmentShader = this.compileShader(this.gl.FRAGMENT_SHADER, fragmentSrc);

    if (!vertexShader || !fragmentShader) {
      console.error("Could not create a shader object!");
      return null;
    }

    var shaderProgram = this.gl.createProgram();

    if (!shaderProgram) {
      console.error("Could not create shader program!");
      return null;
    }

    this.gl.attachShader(shaderProgram, vertexShader);
    this.gl.attachShader(shaderProgram, fragmentShader);
    this.gl.linkProgram(shaderProgram);

    var result = this.gl.getProgramParameter(shaderProgram, this.gl.LINK_STATUS);

    if (!result) {
      var infoLog = this.gl.getProgramInfoLog(shaderProgram);
      console.error("Could not link shader program: " + infoLog);
      return null;
    }

    return shaderProgram;
  }

  bindShader(shader : WebGLProgram | null) : void {
    this.gl.useProgram(shader);
  }

  createTexture() : WebGLTexture | null {

    var texture = this.gl.createTexture();

    if (!texture) {
      console.error("Could not create texture!");
      return null;
    }

    this.gl.bindTexture(this.gl.TEXTURE_2D, texture);
    this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, 1, 1, 0, this.gl.RGBA, this.gl.UNSIGNED_BYTE, new Uint8Array([78, 79, 75, 255])); // Grey colored texture
    this.gl.bindTexture(this.gl.TEXTURE_2D, null);

    return texture;
  }

  copyToTexture(texture : WebGLTexture, image : HTMLImageElement) : void {
    
    this.gl.bindTexture(this.gl.TEXTURE_2D, texture);
    this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.CLAMP_TO_EDGE);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.CLAMP_TO_EDGE);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.LINEAR_MIPMAP_LINEAR);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MAG_FILTER, this.gl.NEAREST);
    this.gl.generateMipmap(this.gl.TEXTURE_2D);
    this.gl.bindTexture(this.gl.TEXTURE_2D, null);
  }

  bindTexture(texture : WebGLTexture | null) : void {
    this.gl.bindTexture(this.gl.TEXTURE_2D, texture);
  }

  setViewport(width : number, height : number) : void {
    this.gl.viewport(0, 0, width, height);
  }

  private compileShader(type : number, source : string) : WebGLShader | null {

    var shader = this.gl.createShader(type);

    if (!shader)  {
      console.error("Could not create shader object!");
      return null;
    }

    this.gl.shaderSource(shader, source);
    this.gl.compileShader(shader);

    var result = this.gl.getShaderParameter(shader, this.gl.COMPILE_STATUS);

    if (!result)  {
      var infoLog = this.gl.getShaderInfoLog(shader);
      console.error("Could not compile shader:", infoLog);

      this.gl.deleteShader(shader);
      return null;
    }

    return shader;
  }

  private getGLType(dataType : DataType) : number {
    
    switch (dataType) {
      case DataType.GL_BYTE: return this.gl.BYTE;
      case DataType.GL_FLOAT: return this.gl.FLOAT;
      default: return -1;
    }
  }

  getContext() : WebGL2RenderingContext {
    return this.gl;
  }
}
