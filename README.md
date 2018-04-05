# Instanced Sprite Renderer for Unity's ECS

This project is a simple example of how Unity's new Entity Component System (ECS) in 2018.1.0b12 can be used to create a performant instanced sprite renderer. Find out about the [new ECS in Unity here](https://github.com/Unity-Technologies/EntityComponentSystemSamples).

The assets used in the example are from [Kenney's Animal Pack](https://kenney.nl/)

## Quick Start
1. Make sure you have [this version of Unity 2018.1.0b12](https://beta.unity3d.com/download/ed1bf90b40e6/public_download.html) installed
2. Make sure the manifest.json file locatated at `.../[PROJECT FOLDER]/Packages/manifest.json` looks like this:
```
{
  "dependencies": {
    "com.unity.entities": "0.0.11"
  },
  "testables": [
    "com.unity.collections",
    "com.unity.entities",
    "com.unity.jobs"
  ],
  "registry": "https://staging-packages.unity.com"
}
```
3. Download [this .unitypackage file](https://github.com/toinfiniityandbeyond/ecs-instanced-sprite-renderer/releases/download/0.1/ECS.Instanced.Sprite.Renderer.unitypackage) and import it.
4. Open **Example Scene** and press play.

## How it Works
By adding `SpriteInstanceRenderer` to an entity it is rendered using its `Position2D` and `Heading2D` as a quad with a texture on it.  The `SpriteInstanceRender` inherits `ISharedComponentData` meaning any entity using same instance of will be drawn in one draw call. This is possible because of [Graphics.DrawMeshInstanced](https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstanced.html) method. In the Example Scene included, 10,000 sprites are drawn. However the before mentioned method only draws a maximum of 1023 instances at once, so it splits up into as many groups necesaary to draw all the instances.

## Improvements
This is a very naive implementation that I threw together, however it does provide fairly good results even with 10,000 entities.
