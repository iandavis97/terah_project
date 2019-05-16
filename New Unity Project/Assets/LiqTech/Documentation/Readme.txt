Copyright Sebastian Strandberg 2017

This is a collection of shaders for objects with screentone-style textures
rendered in screenspace. ScreentonePBR uses a screenspace texture for the
standard shader, while ScreentoneUnlit, ScreentoneDiffuse, and
ScreentoneSpecular are cel shaded.



In general, each texture in a material has the following properties:

Texture					The texture to use for the pattern.

Texture Size			The size of the texture on screen, in pixels.

Texture rotation		Angle of the texture.

Primary color			When two-tone textures are used, white is mapped to this
						color. When using full color textures, this is used to
						tint the texture.

Secondary color			When two-tone textures are used, black is mapped to this color.

Additionally, on the cel-shaded variants:

Brightness threshold	Determines the transition from "base" to "shaded" texures

Gloss power				Exponent used for determining specular component. A higher
						number is sharper.

And for the entire material:

Use two-tone textures	Switch between using a tinted regular texture and a
						black/white texture mapped to primary and secondary
						colors. In the second case, "alpha is grayscale" must
						be set to true on the imported texture.

Double sided			Render the material as double-sided. Useful for clothing.



If you have any questions, email me at sfdstrandberg@gmail.com or contact me
on twitter at @sfdstrandberg