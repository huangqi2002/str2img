using license_str2image_application;
using System;
using static license_str2image_application.License_str2image;

public class Shape
{
  public static void Main()
  {
    string str_license = "民航C3096";
    Supported_Plate_Type plateType = Supported_Plate_Type.LI_AVIATION;
    string out_path = "D:\\workspace\\str_img\\out_img\\LI_AVIATION.png";
    License_str2image license_str2image = new License_str2image(str_license, plateType, out_path);
    license_str2image.paint_plate_image();
  }
}
