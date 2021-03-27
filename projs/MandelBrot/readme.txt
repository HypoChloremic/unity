There are several stuff to note when we create this: 
1. create a RawImage
2. resize the image to fit the canvas
3. create a material
4. create an shader: "image effect shader"
5. make sure that the shader is not Hidden/.., and instead associate it 
   with something that is public, to make it selectable, such as Explorer
6. Select the shader in the created material
7. attach the material to the rawimage material option.
