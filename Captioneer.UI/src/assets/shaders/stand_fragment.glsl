#version 300 es

in highp vec3 normal;
in highp vec3 fragPos;
in highp vec3 lightPos;

out highp vec4 color;

struct Material 
{
    highp vec3 ambient;
    highp vec3 diffuse;
    highp vec3 specular;
    highp float shininess;
};

struct Light
{
    highp float ambient;
    highp float diffuse;
    highp float specular;
};

// Material values for brass as per http://devernay.free.fr/cours/opengl/materials.html
Material material = Material(
    vec3(0.329412, 0.223529, 0.027451),
    vec3(0.780392, 0.568627, 0.113725),
    vec3(0.992157, 0.941176, 0.807843),
    27.9
);
Light light = Light(0.6, 1.0, 1.0);

highp vec3 CalculateAmbientLight()
{
    return light.ambient * material.ambient;
}

highp vec3 CalculateDiffuseLight(highp vec3 normalizedNormal, highp vec3 lightDirection)
{
    return (max(dot(normalizedNormal, lightDirection), 0.0) * material.diffuse) * light.diffuse;
}

highp vec3 CalculateSpecularLight(highp vec3 normalizedNormal, highp vec3 lightDirection)
{
    highp vec3 viewDirection = normalize(lightPos - fragPos);
    highp vec3 reflectDirection = reflect(-lightDirection, normalizedNormal);
    highp float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), material.shininess);

    return (spec * material.specular) * light.specular;
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