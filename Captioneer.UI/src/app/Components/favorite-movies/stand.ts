import { mat4, vec3 } from "gl-matrix";
import { DataType, WebGLService } from "src/app/services/webgl.service";

export class Stand {

    private leftSidePosBuffer! : WebGLBuffer;
    private rightSidePosBuffer! : WebGLBuffer;
    private horizontalPosBuffer! : WebGLBuffer;
    private leftSideVAO! : WebGLVertexArrayObject;
    private rightSideVAO! : WebGLVertexArrayObject;
    private horizontalVAO! : WebGLVertexArrayObject;

    public vertCount! : number;

    private position! : vec3;
    private rotationAngles! : vec3;
    private scale! : vec3;
    private leftSideModelMatrix! : mat4;
    private rightSideModelMatrix! : mat4;
    private horizontalModelMatrix! : mat4;

    constructor(webglService : WebGLService, length : number) { 

        var leftSidePositions = [
            ///Left side/////
           -0.1,  1.5,  0.05,
            0.1,  1.5,  0.05,
            0.1, -1.5,  0.05,
           -0.1, -1.5,  0.05,
           -0.1,  1.5,  0.05,
            0.1, -1.5,  0.05,

            0.1,  1.5, -0.05,
           -0.1,  1.5, -0.05,
           -0.1, -1.5, -0.05,
            0.1, -1.5, -0.05,
            0.1,  1.5, -0.05,
           -0.1, -1.5, -0.05,

           -0.1,  1.5, -0.05,
           -0.1,  1.5,  0.05,
           -0.1, -1.5,  0.05,
           -0.1, -1.5, -0.05,
           -0.1,  1.5, -0.05,
           -0.1, -1.5,  0.05,

            0.1,  1.5,  0.05,
            0.1,  1.5, -0.05,
            0.1, -1.5, -0.05,
            0.1, -1.5,  0.05,
            0.1,  1.5,  0.05,
            0.1, -1.5, -0.05,

           -0.1,  1.5, -0.05,
            0.1,  1.5, -0.05,
            0.1,  1.5,  0.05,
           -0.1,  1.5,  0.05,
           -0.1,  1.5, -0.05,
            0.1,  1.5,  0.05,

           -0.1, -1.5,  0.05,
            0.1, -1.5,  0.05,
            0.1, -1.5, -0.05,
           -0.1, -1.5, -0.05
           -0.1, -1.5,  0.05,
            0.1, -1.5, -0.05,
            /////////////////
        ];

        var rightSidePositions = [
            ////////Right side///////
            length,        1.5,  0.05,
            length + 0.1,  1.5,  0.05,
            length + 0.1, -1.5,  0.05,
            length,       -1.5,  0.05,
            length,        1.5,  0.05,
            length + 0.1, -1.5,  0.05,

            length + 0.1,  1.5, -0.05,
            length,        1.5, -0.05,
            length,       -1.5, -0.05,
            length + 0.1, -1.5, -0.05,
            length + 0.1,  1.5, -0.05,
            length,       -1.5, -0.05,

            length,        1.5, -0.05,
            length,        1.5,  0.05,
            length,       -1.5,  0.05,
            length,       -1.5, -0.05,
            length,        1.5, -0.05,
            length,       -1.5,  0.05,

            length + 0.1,  1.5,  0.05,
            length + 0.1,  1.5, -0.05,
            length + 0.1, -1.5, -0.05,
            length + 0.1, -1.5,  0.05,
            length + 0.1,  1.5,  0.05,
            length + 0.1, -1.5, -0.05,

            length,        1.5, -0.05,
            length + 0.1,  1.5, -0.05,
            length + 0.1,  1.5,  0.05,
            length,        1.5,  0.05,
            length,        1.5, -0.05,
            length + 0.1,  1.5,  0.05,

            length,        -1.5,  0.05,
            length + 0.1, -1.5,  0.05,
            length + 0.1, -1.5, -0.05,
            length,        -1.5, -0.05,
            length,        -1.5,  0.05,
            length + 0.1, -1.5, -0.05,
            ///////////////////////////
        ];

        var horizontalPositions = [
           -0.1,  length,  0.05,
            0.1,  length,  0.05,
            0.1, -1.5,     0.05,
           -0.1, -1.5,     0.05,
           -0.1,  length,  0.05,
            0.1, -1.5,     0.05,

            0.1,  length, -0.05,
           -0.1,  length, -0.05,
           -0.1, -1.5,    -0.05,
            0.1, -1.5,    -0.05,
            0.1,  length, -0.05,
           -0.1, -1.5,    -0.05,

           -0.1,  length, -0.05,
           -0.1,  length,  0.05,
           -0.1, -1.5,     0.05,
           -0.1, -1.5,    -0.05,
           -0.1,  length, -0.05,
           -0.1, -1.5,     0.05,

            0.1,  length,  0.05,
            0.1,  length, -0.05,
            0.1, -1.5,    -0.05,
            0.1, -1.5,     0.05,
            0.1,  length,  0.05,
            0.1, -1.5,    -0.05,

           -0.1,  length, -0.05,
            0.1,  length, -0.05,
            0.1,  length,  0.05,
           -0.1,  length,  0.05,
           -0.1,  length, -0.05,
            0.1,  length,  0.05,

           -0.1, -1.5,     0.05,
            0.1, -1.5,     0.05,
            0.1, -1.5,    -0.05,
           -0.1, -1.5,    -0.05
           -0.1, -1.5,     0.05,
            0.1, -1.5,    -0.05,
        ];

        var leftPosBuffer = webglService.createBuffer(DataType.GL_FLOAT, leftSidePositions);
        var rightPosBuffer = webglService.createBuffer(DataType.GL_FLOAT, rightSidePositions);
        var horizontalPosBuffer = webglService.createBuffer(DataType.GL_FLOAT, horizontalPositions);

        if (!leftPosBuffer || !rightPosBuffer || !horizontalPosBuffer)  {
            console.error("Unable to create one or more position buffers for stand!");
            return;
        }

        this.leftSidePosBuffer = leftPosBuffer;
        this.rightSidePosBuffer = rightPosBuffer;
        this.horizontalPosBuffer = horizontalPosBuffer;

        var leftVAO = webglService.createVAO();
        var rightVAO = webglService.createVAO();
        var horizontalVAO = webglService.createVAO();

        if (!leftVAO || !rightVAO || !horizontalVAO) {
            console.error("Unable to create one or more vertex array objects for stand!");
            return;
        }

        this.leftSideVAO = leftVAO;
        this.rightSideVAO = rightVAO;
        this.horizontalVAO = horizontalVAO;

        webglService.addBuffer(this.leftSideVAO, this.leftSidePosBuffer, 0, 3, DataType.GL_FLOAT);
        webglService.addBuffer(this.rightSideVAO, this.rightSidePosBuffer, 0, 3, DataType.GL_FLOAT);
        webglService.addBuffer(this.horizontalVAO, this.horizontalPosBuffer, 0, 3, DataType.GL_FLOAT);

        this.vertCount = 23;

        this.position = [0.0, 0.0, 0.0];
        this.scale = [1.0, 1.0, 1.0];
        this.rotationAngles = [0.0, 0.0, 0.0];
        this.leftSideModelMatrix = mat4.create();
        this.rightSideModelMatrix = mat4.create();
        this.horizontalModelMatrix = mat4.create();
    }

    public getLeftVAO() : WebGLVertexArrayObject { return this.leftSideVAO; }
    public getRightVAO() : WebGLVertexArrayObject { return this.rightSideVAO; }
    public getHorizontalVAO() : WebGLVertexArrayObject { return this.horizontalVAO; }
    public getVertCount() : number { return this.vertCount; }
    public getPosition() : vec3 { return this.position; }
    public getRotationAngles() : vec3 { return this.rotationAngles; }
    public getScale() : vec3 { return this.scale; }
    public getLeftModelMatrix() : mat4 { return this.leftSideModelMatrix; }
    public getRightModelMatrix() : mat4 { return this.rightSideModelMatrix; }
    public getHorizontalModelMatrix() : mat4 { return this.horizontalModelMatrix; }
}