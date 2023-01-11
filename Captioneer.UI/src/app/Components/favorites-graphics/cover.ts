import { mat4, vec3 } from "gl-matrix";
import { DataType, WebGLService } from "src/app/services/webgl.service";

export class Cover {
    
    private frontBackPositionBuffer! : WebGLBuffer | null;
    private frontBackTexCoordBuffer! : WebGLBuffer | null;
    private sidePositionBuffer! : WebGLBuffer | null;
    private sideTexCoordBuffer! : WebGLBuffer | null;

    private frontBackVAO! : WebGLVertexArrayObject | null;
    private sideVAO! : WebGLVertexArrayObject | null;

    private mainTexture! : WebGLTexture | null;
    private sideTexture! : WebGLTexture | null;

    private frontBackVertCount! : number;
    private sideVertCount! : number;

    private position! : vec3;
    private rotationAngles! : vec3;
    private scale! : vec3;
    private modelMatrix! : mat4;

    constructor(webglService : WebGLService) {

        // 0 - Front, 1 -Back, 2 - Left, 3 - Right, 4 - Top, 5 - Bottom

        var frontBackPositions = [
            // Front
           -1.0,  1.5,  0.05, 0.0,
            1.0,  1.5,  0.05, 0.0,
            1.0, -1.5,  0.05, 0.0,
           -1.0, -1.5,  0.05, 0.0,
           -1.0,  1.5,  0.05, 0.0,
            1.0, -1.5,  0.05, 0.0,

            //Back
            1.0,  1.5, -0.05, 1.0,
           -1.0,  1.5, -0.05, 1.0,
           -1.0, -1.5, -0.05, 1.0,
            1.0, -1.5, -0.05, 1.0,
            1.0,  1.5, -0.05, 1.0,
           -1.0, -1.5, -0.05, 1.0,
        ];
        var sidePositions = [
            //Left
           -1.0,  1.5, -0.05, 2.0,
           -1.0,  1.5,  0.05, 2.0, 
           -1.0, -1.5,  0.05, 2.0,
           -1.0, -1.5, -0.05, 2.0,
           -1.0,  1.5, -0.05, 2.0,
           -1.0, -1.5,  0.05, 2.0,

            //Right
            1.0,  1.5,  0.05, 3.0,
            1.0,  1.5, -0.05, 3.0,
            1.0, -1.5, -0.05, 3.0,
            1.0, -1.5,  0.05, 3.0,
            1.0,  1.5,  0.05, 3.0,
            1.0, -1.5, -0.05, 3.0

            //Top
           -1.0,  1.5, -0.05, 4.0,
            1.0,  1.5, -0.05, 4.0,
            1.0,  1.5,  0.05, 4.0,
           -1.0,  1.5,  0.05, 4.0,
           -1.0,  1.5, -0.05, 4.0,
            1.0,  1.5,  0.05, 4.0,

            //Bottom
           -1.0, -1.5,  0.05, 5.0,
            1.0, -1.5,  0.05, 5.0,
            1.0, -1.5, -0.05, 5.0,
           -1.0, -1.5, -0.05, 5.0,
           -1.0, -1.5,  0.05, 5.0,
            1.0, -1.5, -0.05, 5.0,
        ];

        var frontBackTexCoords = [
            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,
      
            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,
        ];
        var sideTexCoords = [
            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,
      
            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,

            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,
      
            0, 1,
            1, 1,
            1, 0,
            0, 0,
            0, 1,
            1, 0,
        ];

        this.frontBackPositionBuffer = webglService.createBuffer(DataType.GL_FLOAT, frontBackPositions);
        this.frontBackTexCoordBuffer = webglService.createBuffer(DataType.GL_BYTE, frontBackTexCoords);

        this.sidePositionBuffer = webglService.createBuffer(DataType.GL_FLOAT, sidePositions);
        this.sideTexCoordBuffer = webglService.createBuffer(DataType.GL_BYTE, sideTexCoords);

        if (!this.frontBackPositionBuffer || !this.frontBackTexCoordBuffer || !this.sidePositionBuffer || !this.sideTexCoordBuffer) {
            console.error("Unable to create one or more buffer objects for movie cover!");
            return;
        }

        this.frontBackVAO = webglService.createVAO();
        this.sideVAO = webglService.createVAO();

        if (!this.frontBackVAO || !this.sideVAO) {
            console.error("Unable to create one or more VAO for movie cover!");
            return;
        }

        webglService.addBuffer(this.frontBackVAO, this.frontBackPositionBuffer, 0, 4, DataType.GL_FLOAT);
        webglService.addBuffer(this.frontBackVAO, this.frontBackTexCoordBuffer, 1, 2, DataType.GL_BYTE);

        webglService.addBuffer(this.sideVAO, this.sidePositionBuffer, 0, 4, DataType.GL_FLOAT);
        webglService.addBuffer(this.sideVAO, this.sideTexCoordBuffer, 1, 2, DataType.GL_BYTE);

        this.mainTexture = webglService.createTexture();
        this.sideTexture = webglService.createTexture();

        if (!this.mainTexture || !this.sideTexture) {
            console.error("Unable to create one or more texture objects for movie cover!");
            return;
        }

        this.frontBackVertCount = 6 * 2; // 6 vertices per face
        this.sideVertCount = 6 * 4; // 6 vertices per face

        this.position = [-2.5, 0.0, 0.0];
        this.rotationAngles = vec3.create();
        this.scale = [1.0, 1.0, 1.0];
        this.modelMatrix = mat4.create();
    }

    public setPosition(pos : vec3) : void {
        this.position = pos;
        mat4.translate(this.modelMatrix, this.modelMatrix, this.position);
    }

    public setScale(scale : vec3) : void {
        this.scale = scale;
        mat4.scale(this.modelMatrix, this.modelMatrix, scale);
    }

    public setRotation(axis : vec3, degrees : number) : void {
        mat4.rotate(this.modelMatrix, this.modelMatrix, degrees, axis);

        vec3.scale(axis, axis, degrees);
        vec3.add(this.rotationAngles, this.rotationAngles, axis);
    }

    public getFrontBackVAO() : WebGLVertexArrayObject { return this.frontBackVAO!; }
    public getSideVAO() : WebGLVertexArrayObject { return this.sideVAO!; }
    public getMainTex() : WebGLTexture { return this.mainTexture!; }
    public getSideTex() : WebGLTexture { return this.sideTexture!; }
    public getFrontBackVertCount() : number { return this.frontBackVertCount; }
    public getSideVertCount() : number { return this.sideVertCount; }
    public getPosition() : vec3 { return this.position; }
    public getRotationAngles() : vec3 { return this.rotationAngles; }
    public getScale() : vec3 { return this.scale; }
    public getModelMatrix() : mat4 { return this.modelMatrix; }
}