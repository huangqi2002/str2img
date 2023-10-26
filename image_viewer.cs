using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using SkiaSharp;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace license_str2image_application
{
  internal class License_str2image
  {
    //typedefs&enums
    public enum Supported_Plate_Type//车牌种类
    {
      LT_UNKNOWN = 0,   //未知车牌
      LT_BLUE,          //蓝牌小汽车
      LT_BLACK,         //黑牌小汽车
      LT_YELLOW,        //单排黄牌
      LT_YELLOW2,       //双排黄牌（大车尾牌，农用车）
      LT_POLICE,        //警车车牌
      LT_ARMPOL,        //武警车牌
      LT_INDIVI,        //个性化车牌
      LT_ARMY,          //单排军车牌
      LT_ARMY2,         //双排军车牌
      LT_EMBASSY,       //使馆车牌
      LT_HONGKONG,      //香港进出中国大陆车牌
      LT_TRACTOR,       //农用车牌
      LT_COACH,	        //教练车牌
      LT_MACAO,	        //澳门进出中国大陆车牌
      LT_ARMPOL2,       //双层武警车牌
      LT_ARMPOL_ZONGDUI,   // 武警总队车牌
      LT_ARMPOL2_ZONGDUI,  // 双层武警总队车牌
      LI_ENERGY,           //新能源
      LI_AVIATION,         //民航
      LI_ENERGY2,          //大型新能源
      LI_EMERGENCY,        //应急车牌
      LI_CONSULATE         //领事馆
    };

    public enum Supported_Plate_SIZE_Type//车牌种类
    {
      NORMAL_SIZE = 0,   //正常尺寸，440*140
      DOUBLE_DECK_SIZE         //双层尺寸，440*220
    };

    public struct Char_pos_t
    {
      public int left;
      public int right;
      public int top;
      public int bottom;
    }

    //常量
    static string char_db_tab = "藏川鄂甘赣贵桂黑沪吉冀津晋京辽鲁蒙闽宁青琼陕苏皖湘新渝豫粤云浙学警领使港澳0123456789ABCDEFGHJKLMNPQRSTUVWXYZ武民航0123456789";
    //构造函数
    public License_str2image()
    {
      license_str_ = "湘A12345"; //车牌字符串
      plate_type_ = Supported_Plate_Type.LT_BLUE;    //车牌类型
      plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//车牌尺寸类型
      fond_color_ = ("#FFFFFFFF"); //车牌字体颜色
      plate_bitmap_ = null;//车牌模拟图
      out_path_ = "D:\\workspace\\str_img\\out_img\\code-img.png";//输出路径
    }
    public License_str2image(string str_license, Supported_Plate_Type plateType, string out_path)
    {
      license_str_ = str_license;
      plate_type_ = plateType;
      out_path_ = out_path;
      plate_bitmap_ = null;
      plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
      fond_color_ = ("#FFFFFFFF");
    }

    //析构函数
    ~License_str2image()
    {

    }
    //成员函数

    /*读取图片,pstrValue为文件路径，返回位图*/
    protected SKBitmap? Set_attribute(string pstrValue)
    {
      if (pstrValue == null) return null;
      SKBitmap resourceBitmap;//创建位图对象

      using (var input = File.OpenRead(pstrValue))//读取图片文件
      {
        if (pstrValue == null) return null;
        using (var inputStream = new SKManagedStream(input))//新建一个输入流
        {
          if (inputStream == null) return null;
          resourceBitmap = SKBitmap.Decode(inputStream);//加载到位图中
        }
      }
      return resourceBitmap;
    }
    //设置车牌底图
    protected void Paint_bk_image()
    {
      using (var original = new SKBitmap())
      {

      }
      //设置车牌种类底图
      switch (plate_type_)
      {
        case Supported_Plate_Type.LT_BLUE://蓝牌小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_blue.png");//设置底图
          fond_color_ = ("#FFFFFFFF");//字体颜色
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//设置尺寸类型
          break;
        case Supported_Plate_Type.LT_BLACK://黑牌小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_black.jpg");
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_YELLOW://单排黄牌小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_yellow.png");
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_YELLOW2://双排黄牌小汽车
          break;
        case Supported_Plate_Type.LT_POLICE://单排警车小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_white.jpg");
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_ARMPOL://单排武警小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_white.jpg");
          fond_color_ = ("#FFFF0000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_INDIVI://个性化车牌
          break;
        case Supported_Plate_Type.LT_ARMY://单排军车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_white.jpg");
          fond_color_ = ("#FFFF0000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_ARMY2://双排军车牌
          break;
        case Supported_Plate_Type.LT_EMBASSY://使馆车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_black.jpg");
          fond_color_ = ("#FFFF0000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_HONGKONG://香港进出中国大陆车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_black.jpg");
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_TRACTOR://农用车牌
          break;
        case Supported_Plate_Type.LT_COACH://教练车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_yellow.png");
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_MACAO://澳门进出中国大陆车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_black.jpg");
          fond_color_ = ("#FFFFFFFF");
          break;
        case Supported_Plate_Type.LT_ARMPOL2://双层武警车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL_ZONGDUI:// 武警总队车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL2_ZONGDUI:// 双层武警总队车牌
          break;
        case Supported_Plate_Type.LI_AVIATION://民航
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_green.png");
          fond_color_ = ("#FFFFFFFF");
          break;
        case Supported_Plate_Type.LI_ENERGY://新能源小汽车
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_new_energy.png");
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LI_ENERGY2://大型新能源
          break;
        case Supported_Plate_Type.LI_EMERGENCY://应急车牌
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_white.jpg");
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LI_CONSULATE://领事馆
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_black.jpg");
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        default:
          plate_bitmap_ = Set_attribute("D:\\workspace\\str_img\\img\\civil_blue.png");//设置底图
          fond_color_ = ("#FFFFFFFF");//字体颜色
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//设置尺寸类型
          break;
      }
      //resize
      if (plate_bitmap_ == null) return;
      if(plate_size_type_ == Supported_Plate_SIZE_Type.NORMAL_SIZE){
        plate_bitmap_ = plate_bitmap_.Resize(new SKImageInfo(440, 140), SKFilterQuality.High);//resize
      }
      else{
        plate_bitmap_ = plate_bitmap_.Resize(new SKImageInfo(440, 220), SKFilterQuality.High);//resize
      }
    }
    //根据字符找出在图中的位置
    protected SKRect read_char_pos(char license_c)
    {
      int find_idx = char_db_tab.IndexOf(license_c);

      int img_size_x = 224;//图像整体尺寸
      //int img_size_y = 550;
      int word_size_x = 28;//单个字符尺寸
      int word_size_y = 50;

      int char_db_row_elem_num = img_size_x / word_size_x;//单行字符数量
      int x_idx = find_idx % char_db_row_elem_num;//字符列数
      int y_idx = find_idx / char_db_row_elem_num;//字符行数

      //字符所在位置
      float char_pos_left = x_idx * word_size_x;
      float char_pos_right = (x_idx + 1) * word_size_x;
      float char_pos_top = y_idx * word_size_y;
      float char_pos_bottom = (y_idx + 1) * word_size_y;
      SKRect char_pos = new SKRect(char_pos_left, char_pos_top, char_pos_right, char_pos_bottom);

      return char_pos;
    }
    //绘制车牌号单个字符
    protected void Paint_char_image(SKRect source_char_pos, SKRect dest_char_pos, string? fond_color)
    {
      using (var input = File.OpenRead("D:\\workspace\\str_img\\img\\civil_db_28x50.jpg"))//读取图片文件
      {
        if (input == null) return;
        using (var inputStream = new SKManagedStream(input))//新建一个输入流
        {
          if (inputStream == null) return;
          SKBitmap resourceBitmap = SKBitmap.Decode(inputStream);//加载到位图中
          //绘制
          //创建SKCanvas对象
          using (SKCanvas canvas = new SKCanvas(plate_bitmap_))
          {
            using (SKPaint paint = new SKPaint())//颜色设置
            {
              string paint_color = fond_color;
              float color_a = Convert.ToInt32(fond_color.Substring(1, 2), 16);
              float color_r = Convert.ToInt32(fond_color.Substring(3, 2), 16);
              float color_g = Convert.ToInt32(fond_color.Substring(5, 2), 16);
              float color_b = Convert.ToInt32(fond_color.Substring(7, 2), 16);

              paint.ColorFilter =
              SKColorFilter.CreateColorMatrix(new float[]
              {
                color_r/255, 0f, 0f, 0, 0,
                color_g/255, 0f, 0f, 0, 0,
                color_b/255, 0f, 0f, 0, 0,
                color_a/255, 0, 0, 0, 0
              });
              canvas.DrawBitmap(resourceBitmap, source_char_pos, dest_char_pos, paint);
              Save_bitmap(plate_bitmap_);
            }
              
          }
        }
      }
    }
    //绘制车牌
    protected void Paint_license_image()
    {
      if(license_str_ == null || plate_bitmap_ == null) return;
      //对武警
      if (license_str_[0] == 'W' && license_str_[1] == 'J')
      {
        license_str_ = license_str_.Remove(0,2).Insert(0, '武'.ToString());
      }
      Console.WriteLine("str_license_： {0}, plate_type_： {1}", license_str_, plate_type_);
      //计算每个字符在模拟车牌上的位置
      int standW = 440;
      int standH = 140;
      int tab_start = 38;//七字符
      int license_width = 57;
      int espase_width = 34;
      int word_width = 45;
      int word_height = 90;
      if (plate_size_type_ == Supported_Plate_SIZE_Type.NORMAL_SIZE)//单层车牌
      {
        if (license_str_.Length == 8)//八字符
        {
          license_width = 47;
          espase_width = 44;
          word_width = 35;
        }
      }
      int[] wtab = new int[8] {
          tab_start,
          (tab_start+license_width),
          (tab_start+license_width+word_width+espase_width),
          (tab_start+license_width+word_width+espase_width+license_width),
          (tab_start+license_width+word_width+espase_width+license_width*2),
          (tab_start+license_width+word_width+espase_width+license_width*3),
          (tab_start+license_width+word_width+espase_width+license_width*4),
          (tab_start+license_width+word_width+espase_width+license_width*5)};
      switch (plate_type_)
      {
        case Supported_Plate_Type.LT_POLICE://单排警车小汽车
          wtab[1] = tab_start + word_width + espase_width;
          break;
        case Supported_Plate_Type.LT_INDIVI://个性化车牌
          break;
        case Supported_Plate_Type.LT_ARMY2://双排军车牌
          break;
        case Supported_Plate_Type.LT_EMBASSY://使馆车牌
          wtab[1] = tab_start + license_width;
          wtab[2] = tab_start + license_width * 2;
          wtab[3] = tab_start + license_width * 3;
          break;
        case Supported_Plate_Type.LT_TRACTOR://农用车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL2://双层武警车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL_ZONGDUI:// 武警总队车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL2_ZONGDUI:// 双层武警总队车牌
          break;
        case Supported_Plate_Type.LI_AVIATION://民航
          break;
        case Supported_Plate_Type.LI_ENERGY://新能源小汽车
          break;
        case Supported_Plate_Type.LI_ENERGY2://大型新能源
          break;
        case Supported_Plate_Type.LI_EMERGENCY://应急车牌
          wtab[1] = tab_start + word_width + espase_width;
          break;
        case Supported_Plate_Type.LI_CONSULATE://领事馆
          wtab[1] = tab_start + license_width;
          wtab[2] = tab_start + license_width * 2;
          wtab[3] = tab_start + license_width * 3;
          break;
        default:
          break;
      }
      int temp_index = 0;
      foreach (char license_c in license_str_) {
        //得到字符在字符图中位置
        SKRect source_char_pos = read_char_pos(license_c);
        //计算字符在模拟车牌图中的位置
        float dest_char_pos_left = wtab[temp_index] - word_width / 2;
        float dest_char_pos_right = wtab[temp_index] + word_width / 2;
        float dest_char_pos_top = standH/2 - word_height / 2;
        float dest_char_pos_bottom = standH / 2 + word_height / 2;
        SKRect dest_char_pos = new SKRect(dest_char_pos_left, dest_char_pos_top, dest_char_pos_right, dest_char_pos_bottom);
        //根据位置绘制车牌字符
        if(fond_color_ == null) { fond_color_ = "#FFFFFFFF"; }
        string paint_color = fond_color_;
        switch (plate_type_)
        {
          case Supported_Plate_Type.LT_YELLOW2://双排黄牌小汽车
            break;
          case Supported_Plate_Type.LT_POLICE://单排警车小汽车
            if(license_c == '警')
            {
              paint_color = "#FFFF0000";
            }
            break;
          case Supported_Plate_Type.LT_ARMPOL://单排武警小汽车
            if(temp_index == 1)
            {
              fond_color_ = "#FF000000";
            }
            break;
          case Supported_Plate_Type.LT_INDIVI://个性化车牌
            break;
          case Supported_Plate_Type.LT_ARMY://单排军车牌
            if (temp_index == 1)
            {
              fond_color_ = "#FF000000";
            }
            break;
          case Supported_Plate_Type.LT_ARMY2://双排军车牌
            break;
          case Supported_Plate_Type.LT_EMBASSY://使馆车牌
            if (temp_index == 0)
            {
              fond_color_ = "#FFFFFFFF";
            }
            break;
          case Supported_Plate_Type.LT_TRACTOR://农用车牌
            break;
          case Supported_Plate_Type.LT_ARMPOL2://双层武警车牌
            break;
          case Supported_Plate_Type.LT_ARMPOL_ZONGDUI:// 武警总队车牌
            break;
          case Supported_Plate_Type.LT_ARMPOL2_ZONGDUI:// 双层武警总队车牌
            break;
          case Supported_Plate_Type.LI_AVIATION://民航
            break;
          case Supported_Plate_Type.LI_ENERGY://新能源小汽车
            break;
          case Supported_Plate_Type.LI_ENERGY2://大型新能源
            break;
          case Supported_Plate_Type.LI_EMERGENCY://应急车牌
            if (temp_index == 1 || license_c == '应' || license_c == '急')
            {
              paint_color = "#FFFF0000";
            }
            break;
          default:
            break;
        }
        Paint_char_image(source_char_pos, dest_char_pos, paint_color);
        ++temp_index;
      }
    }
    protected void Save_bitmap(SKBitmap? plate_bitmap)//保存图片
    {
      if (plate_bitmap == null || out_path_ == null) return;
      using (var writeStream = System.IO.File.OpenWrite(out_path_))
      {
        plate_bitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(writeStream);
      }
    }
    public void paint_plate_image()//合成模拟图
    {
      Console.WriteLine("str_license_： {0}, plate_type_： {1}", license_str_, plate_type_);
      Paint_bk_image();//设置车牌属性
      Paint_license_image();//设置车牌字符图
      Save_bitmap(plate_bitmap_);//保存车牌模拟图
    }
    //成员变量
    private string? license_str_; //车牌号
    private Supported_Plate_Type plate_type_;//车牌种类
    Supported_Plate_SIZE_Type plate_size_type_;//车牌尺寸类别
    private SKBitmap? plate_bitmap_;//车牌模拟图
    private string? fond_color_;//车牌字体颜色
    private string? out_path_;//输出路径
  }
}