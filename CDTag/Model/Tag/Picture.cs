using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CDTag.Common;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace CDTag.Model.Tag
{
    public class Picture : ModelBase
    {
        public Picture(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            PictureType = PictureType.CoverFront;

            byte[] pictureData = File.ReadAllBytes(path);
            PictureBytes = pictureData;
            try
            {
                MemoryStream memoryStream = new MemoryStream(pictureData);
                memoryStream.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                ImageSource = bitmapImage;
            }
            catch (NotSupportedException)
            {
            }
        }

        public Picture(IAttachedPicture attachedPicture)
        {
            if (attachedPicture == null)
                throw new ArgumentNullException("attachedPicture");

            AttachedPicture = attachedPicture;

            Description = attachedPicture.Description;
            PictureType = attachedPicture.PictureType;

            byte[] pictureData = attachedPicture.PictureData;
            PictureBytes = pictureData;
            if (pictureData != null)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream(pictureData);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();

                    ImageSource = bitmapImage;
                }
                catch (NotSupportedException)
                {
                }
            }
        }

        public ImageSource ImageSource
        {
            get { return Get<ImageSource>(); }
            set { Set(value); }
        }

        public string Description
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public PictureType PictureType
        {
            get { return Get<PictureType>(); }
            set { Set(value); }
        }

        public byte[] PictureBytes 
        {
            get { return Get<byte[]>(); }
            private set { Set(value); }
        }

        public IAttachedPicture AttachedPicture
        {
            get { return Get<IAttachedPicture>(); }
            private set { Set(value); }
        }
    }
}
