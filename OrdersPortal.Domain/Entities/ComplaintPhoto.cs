using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintPhoto
    {

        [Key]
        public int ComplaintPhotoId { get; set; }

        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }
        public byte[] Photo { get; set; }

        public string PhotoName { get; set;}

        public Boolean UploadComplete { get; set; }

        public byte[] PhotoIco { get; set; }

        public int PhotoSize { get; set; }

        public void AddThumbnail()
        {
            //---------- Getting the Image File
            //MemoryStream eee = new MemoryStream(this.Photo);
            System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(this.Photo));
                

            //---------- Getting Size of Original Image
            double imgHeight = img.Size.Height;
            double imgWidth = img.Size.Width;

            //---------- Getting Decreased Size
            double x = imgWidth / 100;
            int newWidth = Convert.ToInt32(imgWidth / x);
            int newHeight = Convert.ToInt32(imgHeight / x);

            //---------- Creating Small Image
            System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            System.Drawing.Image myThumbnail = img.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);

            //------------------ конвертуємо в byte[] 
            using (var ms = new MemoryStream())
            {
               myThumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
               this.PhotoIco = ms.ToArray();
            }
        }
        public bool ThumbnailCallback()
        {
            return false;
        }

    }
}