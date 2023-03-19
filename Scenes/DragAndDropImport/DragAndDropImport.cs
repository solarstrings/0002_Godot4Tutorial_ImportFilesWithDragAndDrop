using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class DragAndDropImport : Control
{
    private Sprite2D _image;
    private Label _textLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textLabel = GetNode<Label>("VBoxContainer/Panel/Text");    // Get the text label node
        _image = GetNode<Sprite2D>("VBoxContainer/Panel/Image");    // Get the Sprite2D image node		
        ConnectFilesDroppedSignal();
    }

    private bool FileIsText(string fileName)
    {
        // If the file name ends with .txt
        if (Path.GetExtension(fileName).ToLower().Equals(".txt"))
        {
            return true;    // Return true, it's a text file
        }
        // if not, return false
        return false;
    }

    private bool FileIsImage(string fileName)
    {
        // The list of valid image extensions
        List<string> imageExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".svg", ".svgz", ".tga", ".webp" };

        // Loop through the valid extensions
        foreach (var image in imageExtensions)
        {
            // If the file name ends with one of the extensions
            if (Path.GetExtension(fileName).ToLower().Equals(image))
            {
                return true;    // return true
            }
        }
        // Return false, the file is not an image
        return false;
    }


    private void ConnectFilesDroppedSignal()
    {
        var root = GetTree().Root;              // Get the root node
        root.FilesDropped += OnFilesDropped;    // Run the OnFilesDropped method whenever the signal is triggered
    }

    private void LoadSprite2DTextureFromDisk(string imagePath, Sprite2D sprite)
    {
        Image img = new Image();

        // If the image could be loaded
        if (img.Load(imagePath) == Error.Ok)
        {
            var texture = ImageTexture.CreateFromImage(img);            // Create a texture from the image
            sprite.Texture = texture;                                   // Set the sprite texture
            sprite.Centered = false;                                    // Make the sprite draw from the top-left corner
        }
        // If the image loding failed
        else
        {
            _textLabel.Text = "Could not load the image:" + imagePath;  // Show the user what went wrong
        }
    }

    public void OnFilesDropped(String[] files)
    {
        _textLabel.Text = string.Empty;                     // Clear the text whenever a file is dropped
        // If the file is a text file
        if (FileIsText(files[0]))
        {
            string text = File.ReadAllText(files[0]);       // Read all the text and store it as string
            _textLabel.Text = text;                         // Set the label text
        }
        // If the file is an image
        else if (FileIsImage(files[0]))
        {
            LoadSprite2DTextureFromDisk(files[0], _image);   // Load the texture from disk and apply it to the Sprite2D node
        }
        else
        {
            // Tell the user that the file type is not supported
            _textLabel.Text = "\"" + Path.GetFileName(files[0]) + "\"" + " Is not a .txt or supported image file";
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
