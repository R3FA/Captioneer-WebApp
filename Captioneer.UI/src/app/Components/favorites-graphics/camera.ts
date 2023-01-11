import { mat4, vec3 } from "gl-matrix";

export class Camera {

    private position! : vec3;
    private front! : vec3;
    private up! : vec3;
    
    private projMatrix! : mat4;
    private viewMatrix! : mat4;
    private inverseViewProjMatrix! : mat4;

    private cameraSpeed! : number;
    private lockedMovement! : boolean;

    constructor() {
        this.position = [0.0, 0.0, 0.0];
        this.front = [0.0, 0.0, -1.0];
        this.up = [0.0, 1.0, 0.0];
        this.projMatrix = mat4.create();
        this.viewMatrix = mat4.create();
        this.inverseViewProjMatrix = mat4.create();
        this.cameraSpeed = 1.0;
        this.lockedMovement = true;

        this.updateView();
    }

    private updatePerspective(fov : number, aspectRatio : number, nearClip : number, farClip : number) : void {
        mat4.perspective(this.projMatrix, fov * (Math.PI / 180.0), aspectRatio, nearClip, farClip);
    }

    private updateView() : void {
        let direction = vec3.create();
        vec3.add(direction, this.position, this.front);
        mat4.lookAt(this.viewMatrix, this.position, direction, this.up);
    }

    private updateViewProjInverse() : void {
        let viewProj = mat4.create();
        mat4.multiply(viewProj, this.projMatrix, this.viewMatrix);
        mat4.invert(this.inverseViewProjMatrix, viewProj);
    }

    public onUpdate() : void {
        this.updateView();
        this.updateViewProjInverse();
    }

    public onResize(fov : number, aspectRatio : number, nearClip : number, farClip : number) : void {
        this.updatePerspective(fov, aspectRatio, nearClip, farClip);
        this.updateViewProjInverse();
    }

    public onMouseEvent(event : MouseEvent, deltaTime : number, allowedTraversal : number) : void {
        
        if (event.type == "mouseleave") {
            this.lockedMovement = true;
        }
        else if (event.type == "mousedown" && event.button == 0) {
            this.lockedMovement = false;
        }
        else if (event.type == "mouseup" && event.button == 0) {
            this.lockedMovement = true;
        }

        if (!this.lockedMovement) {

            if (event.movementX > 0) {

                if (this.position[0] <= -2.5) {
                    return;
                }

                let change = vec3.create();
                vec3.cross(change, this.front, this.up);
                vec3.normalize(change, change);
                vec3.scale(change, change, event.movementX * this.cameraSpeed * deltaTime);
                vec3.subtract(this.position, this.position, change);
            }
            else {

                if (this.position[0] >= allowedTraversal) {
                    return;
                }

                let change = vec3.create();
                vec3.cross(change, this.front, this.up);
                vec3.normalize(change, change);
                vec3.scale(change, change, Math.abs(event.movementX) * this.cameraSpeed * deltaTime);
                vec3.add(this.position, this.position, change);
            }
        }
    }

    public getPosition() : vec3 { return this.position; }
    public getFront() : vec3 { return this.front; }
    public getUp() : vec3 { return this.up; }
    public getProjMatrix() : mat4 { return this.projMatrix; }
    public getViewMatrix() : mat4 { return this.viewMatrix; }
    public getInverseViewProjMatrix() : mat4 { return this.inverseViewProjMatrix; }
    public isLocked() : boolean { return this.lockedMovement; }
} 