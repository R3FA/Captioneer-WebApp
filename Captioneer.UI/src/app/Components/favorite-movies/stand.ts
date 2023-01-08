import { mat4, vec3 } from "gl-matrix";
import { DataType, WebGLService } from "src/app/services/webgl.service";

export class Stand {

    private positionBuffer! : WebGLBuffer;
    private vao! : WebGLVertexArrayObject;

    private vertCount! : number;
    private position! : vec3;
    private rotationAngles! : vec3;
    private scale! : vec3;
    private modelMatrix! : mat4;

    constructor(webglService : WebGLService, length : number) { 

        var positions = [
           0,  length,  0.05,
            2,  length,  0.05,
            2, -1.5,     0.05,
           0, -1.5,     0.05,
           0,  length,  0.05,
            2, -1.5,     0.05,

            2,  length, -0.05,
           0,  length, -0.05,
           0, -1.5,    -0.05,
            2, -1.5,    -0.05,
            2,  length, -0.05,
           0, -1.5,    -0.05,

           0,  length, -0.05,
           0,  length,  0.05,
           0, -1.5,     0.05,
           0, -1.5,    -0.05,
           0,  length, -0.05,
           0, -1.5,     0.05,

            2,  length,  0.05,
            2,  length, -0.05,
            2, -1.5,    -0.05,
            2, -1.5,     0.05,
            2,  length,  0.05,
            2, -1.5,    -0.05,

           0,  length, -0.05,
            2,  length, -0.05,
            2,  length,  0.05,
           0,  length,  0.05,
           0,  length, -0.05,
            2,  length,  0.05,

           0, -1.5,     0.05,
            2, -1.5,     0.05,
            2, -1.5,    -0.05,
           0, -1.5,    -0.05,
           0, -1.5,     0.05,
            2, -1.5,    -0.05,
        ];

        var posBuffer = webglService.createBuffer(DataType.GL_FLOAT, positions);

        if (!posBuffer)  {
            console.error("Unable to create one or more position buffers for stand!");
            return;
        }

        this.positionBuffer = posBuffer;

        var vao = webglService.createVAO();

        if (!vao) {
            console.error("Unable to create one or more vertex array objects for stand!");
            return;
        }

        this.vao = vao;

        webglService.addBuffer(this.vao, this.positionBuffer, 0, 3, DataType.GL_FLOAT);

        this.vertCount = 23;

        this.position = [0.0, 0.0, 0.0];
        this.scale = [1.0, 1.0, 1.0];
        this.rotationAngles = [0.0, 0.0, 0.0];
        this.modelMatrix = mat4.create();
    }

    public setPosition(pos : vec3) : void {
        this.position = pos;
        mat4.translate(this.modelMatrix, this.modelMatrix, pos);
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

    public getVAO() : WebGLVertexArrayObject { return this.vao; }
    public getVertCount() : number { return this.vertCount; }
    public getPosition() : vec3 { return this.position; }
    public getRotationAngles() : vec3 { return this.rotationAngles; }
    public getScale() : vec3 { return this.scale; }
    public getModelMatrix() : mat4 { return this.modelMatrix; }
}