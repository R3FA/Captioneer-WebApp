#version 300 es

in highp vec2 tex;
in highp vec3 normal;
in highp vec3 fragPos;
in highp vec3 lightPos;

out highp vec4 color;

struct Material 
{
    sampler2D diffuse;
    highp vec3 specular;
    highp float shininess;
};

struct Light
{
    highp float ambient;
    highp float diffuse;
    highp float specular;
};

uniform Material uMaterial;

Light light = Light(0.1, 1.0, 0.5);

highp vec3 CalculateAmbientLight()
{
    return light.ambient * vec3(texture(uMaterial.diffuse, tex));
}

highp vec3 CalculateDiffuseLight(highp vec3 normalizedNormal, highp vec3 lightDirection)
{
    highp float diff = max(dot(normalizedNormal, lightDirection), 0.0);

    return light.diffuse * diff * vec3(texture(uMaterial.diffuse, tex));
}

highp vec3 CalculateSpecularLight(highp vec3 normalizedNormal, highp vec3 lightDirection)
{
    highp vec3 viewDirection = normalize(lightPos - fragPos);
    highp vec3 reflectDirection = reflect(-lightDirection, normalizedNormal);
    highp float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), uMaterial.shininess);

    return (spec * uMaterial.specular) * light.specular;
}

void main()
{
    highp vec3 normalizedNormal = normalize(normal);
    highp vec3 lightDirection = normalize(lightPos - fragPos);

    highp vec3 ambient = CalculateAmbientLight();
    highp vec3 diffuse = CalculateDiffuseLight(normalizedNormal, lightDirection);
    highp vec3 specular = CalculateSpecularLight(normalizedNormal, lightDirection);

    color = vec4(ambient + diffuse + specular, 1.0);
}