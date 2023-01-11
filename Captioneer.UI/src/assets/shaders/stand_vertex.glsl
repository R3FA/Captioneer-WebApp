#version 300 es

layout(location = 0) in vec4 aPos;

out vec3 normal;
out vec3 fragPos;
out vec3 lightPos;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProj;
uniform vec2 uLightPos;

vec3 CalculateNormal()
{
    switch(int(aPos.w))
    {
        case 0:
            return vec3(0.0, 0.0, 1.0);
        case 1:
            return vec3(0.0, 0.0, -1.0);
        case 2:
            return vec3(-1.0, 0.0, 0.0);
        case 3:
            return vec3(1.0, 0.0, 0.0);
        case 4:
            return vec3(0.0, 1.0, 0.0);
        case 5:
            return vec3(0.0, -1.0, 0.0);
    }
}

void main()
{
    gl_Position = uProj * uView * uModel * vec4(aPos.xyz, 1.0);
    normal = CalculateNormal();
    fragPos = vec3(uModel * vec4(aPos.xyz, 1.0));
    lightPos = vec3(uLightPos, -1.0);
}