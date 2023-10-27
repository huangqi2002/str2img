using license_str2image_application;
using SkiaSharp;
using System;
using static license_str2image_application.License_str2image;

public class Shape
{
  public static void Main()
  {
    //蓝牌小汽车 LT_BLUE
    string str_license = "湘A12345";
    Supported_Plate_Type plateType = Supported_Plate_Type.LT_BLUE;
    string out_path = Path.Combine(Environment.CurrentDirectory, "out_img");
    string in_path = Path.Combine(Environment.CurrentDirectory, "img");//图片存储文件夹
    License_str2image license_str2image = new License_str2image(str_license, plateType, out_path, in_path);
    license_str2image.paint_plate_image();

    /*string new_char_db_tab_normal = "藏川鄂甘赣贵桂黑沪吉冀津晋京辽鲁蒙闽宁青琼陕苏皖湘新渝豫粤云浙学警领使港澳0123456789ABCDEFGHJKLMNPQRSTUVWXYZ武民航应急";
    string add_char; 
    for (int i = 0;i< new_char_db_tab_normal.Length;i++)
    {
      Console.WriteLine("{0}", i);
      add_char = "140_" + new_char_db_tab_normal[i] + ".jpg";
      string add_char_p = Path.Combine(in_path, "font_model", add_char);
      license_str2image.add_char_image(Path.Combine(in_path, "civil_db_112x200.jpg"), add_char_p, i);
    }

    string new_char_db_tab_energy = "0123456789";
    for (int i = 0; i < new_char_db_tab_energy.Length; i++)
    {
      Console.WriteLine("{0}", i);
      add_char = "green_" + new_char_db_tab_energy[i] + ".jpg";
      string add_char_p = Path.Combine(in_path, "font_model", add_char);
      license_str2image.add_char_image(Path.Combine(in_path, "civil_db_112x200.jpg"), add_char_p, i + new_char_db_tab_normal.Length);
    }*/



    //license_str2image.add_char_image(Path.Combine(in_path, "civil_db_112x200.jpg"), Path.Combine(in_path, "ying.png"), 84);
    //license_str2image.add_char_image(Path.Combine(in_path, "civil_db_112x200.jpg"), Path.Combine(in_path, "ji.png"), 85);

    //黑牌小汽车 LT_BLACK
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_BLACK;
    license_str2image.paint_plate_image();

    //单排黄牌 LT_YELLOW
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_YELLOW;
    license_str2image.paint_plate_image();

    //双排黄牌 LT_YELLOW2
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_YELLOW2;
    license_str2image.paint_plate_image();

    //警车车牌 LT_POLICE
    license_str2image.License_str_ = "湘A1234警";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_POLICE;
    license_str2image.paint_plate_image();

    //武警车牌 LT_ARMPOL
    license_str2image.License_str_ = "WJK12345";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_ARMPOL;
    license_str2image.paint_plate_image();

    //个性化车牌 LT_INDIVI
    license_str2image.License_str_ = "KA12345";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_INDIVI;
    license_str2image.paint_plate_image();

    //单排军车牌 LT_ARMY
    license_str2image.License_str_ = "KA12345";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_ARMY;
    license_str2image.paint_plate_image();

    //双排军车牌 LT_ARMY2
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_ARMY2;
    license_str2image.paint_plate_image();

    //使馆车牌 LT_EMBASSY
    license_str2image.License_str_ = "224578使";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_EMBASSY;
    license_str2image.paint_plate_image();

    //香港进出中国大陆车牌 LT_HONGKONG
    license_str2image.License_str_ = "粤Z1234港";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_HONGKONG;
    license_str2image.paint_plate_image();

    //农用车牌 LT_TRACTOR
    license_str2image.License_str_ = "京0423456";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_TRACTOR;
    license_str2image.paint_plate_image();

    //教练车牌 LT_COACH
    license_str2image.License_str_ = "湘A1234学";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_COACH;
    license_str2image.paint_plate_image();

    //澳门进出中国大陆车牌 LT_MACAO
    license_str2image.License_str_ = "粤Z1234澳";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_MACAO;
    license_str2image.paint_plate_image();

    //双层武警车牌 LT_ARMPOL2
    license_str2image.License_str_ = "WJK12345";
    license_str2image.Plate_type_ = Supported_Plate_Type.LT_ARMPOL2;
    license_str2image.paint_plate_image();

    // 武警总队车牌 LT_ARMPOL_ZONGDUI

    // 双层武警总队车牌 LT_ARMPOL2_ZONGDUI

    //民航 LI_AVIATION
    license_str2image.License_str_ = "民航C3096";
    license_str2image.Plate_type_ = Supported_Plate_Type.LI_AVIATION;
    license_str2image.paint_plate_image();

    //新能源 LI_ENERGY
    license_str2image.License_str_ = "湘A12345D";
    license_str2image.Plate_type_ = Supported_Plate_Type.LI_ENERGY;
    license_str2image.paint_plate_image();

    //领事馆 LI_CONSULATE*/
    license_str2image.License_str_ = "湘22478领";
    license_str2image.Plate_type_ = Supported_Plate_Type.LI_CONSULATE;
    license_str2image.paint_plate_image();

    //大型新能源 LI_ENERGY2
    license_str2image.License_str_ = "湘A12345D";
    license_str2image.Plate_type_ = Supported_Plate_Type.LI_ENERGY2;
    license_str2image.paint_plate_image();

    //应急车牌 LI_EMERGENCY
    license_str2image.License_str_ = "湘A2234应急";
    license_str2image.Plate_type_ = Supported_Plate_Type.LI_EMERGENCY;
    license_str2image.paint_plate_image();
  }
}
