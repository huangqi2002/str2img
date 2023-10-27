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
using System.Diagnostics;

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
      LI_EMERGENCY,        //LONG_SIZE车牌
      LI_CONSULATE         //领事馆
    };

    public enum Supported_Plate_SIZE_Type//车牌种类
    {
      NORMAL_SIZE = 0,   //正常尺寸，440*140
      LONG_SIZE,   //新能源尺寸，480*140
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
    static string char_db_tab = "藏川鄂甘赣贵桂黑沪吉冀津晋京辽鲁蒙闽宁青琼陕苏皖湘新渝豫粤云浙学警领使港澳0123456789ABCDEFGHJKLMNPQRSTUVWXYZ武民航应急";
    //构造函数
    public License_str2image()
    {
      license_str_ = "湘A12345"; //车牌字符串
      plate_type_ = Supported_Plate_Type.LT_BLUE;    //车牌类型
      plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//车牌尺寸类型
      fond_color_ = ("#FFFFFFFF"); //车牌字体颜色
      plate_bitmap_ = null;//车牌模拟图
      out_path_ = "out_img\\code-img.png";//输出路径
      in_path_ = Path.Combine(Environment.CurrentDirectory, "img");//图片路径
      is_is_sigle_char_img_ = true;//只有一张字符集图
    }
    public License_str2image(string str_license, Supported_Plate_Type plateType, string out_path,string in_path = null)
    {
      if(in_path == null) in_path_ = Path.Combine(Environment.CurrentDirectory, "img");
      license_str_ = str_license;
      if (plateType == Supported_Plate_Type.LT_UNKNOWN || plateType == Supported_Plate_Type.LT_INDIVI ||
         plateType == Supported_Plate_Type.LT_ARMPOL_ZONGDUI || plateType == Supported_Plate_Type.LT_ARMPOL2_ZONGDUI)
        return;
      plate_type_ = plateType;
      out_path_ = out_path;
      plate_bitmap_ = null;
      plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
      fond_color_ = ("#FFFFFFFF");
      in_path_ = in_path;
      is_is_sigle_char_img_ = true;//只有一张字符集图
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
      if (in_path_ == null) return;
      //设置车牌种类底图
      switch (plate_type_)
      {
        case Supported_Plate_Type.LT_BLUE://蓝牌小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_blue.png"));
          fond_color_ = ("#FFFFFFFF");//字体颜色
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//设置尺寸类型
          break;

        case Supported_Plate_Type.LT_BLACK://黑牌小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_black.png"));
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;

        case Supported_Plate_Type.LT_YELLOW://单排黄牌小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_yellow.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;

        case Supported_Plate_Type.LT_YELLOW2://双排黄牌小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_yellow2.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE;
          break;
        case Supported_Plate_Type.LT_POLICE://单排警车小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white.jpg"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_ARMPOL://单排武警小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white.jpg"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_INDIVI://个性化车牌
          break;
        case Supported_Plate_Type.LT_ARMY://单排军车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white.jpg"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_ARMY2://双排军车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white2.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE;
          break;
        case Supported_Plate_Type.LT_EMBASSY://使馆车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_black.png"));
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_HONGKONG://香港进出中国大陆车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_black.png"));
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_TRACTOR://农用车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_green.png"));
          plate_size_type_ = Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE;
          fond_color_ = ("#FFFFFFFF");
          break;
        case Supported_Plate_Type.LT_COACH://教练车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_yellow.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LT_MACAO://澳门进出中国大陆车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_black.png"));
          fond_color_ = ("#FFFFFFFF");
          break;
        case Supported_Plate_Type.LT_ARMPOL2://双层武警车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white2.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE;
          break;
        case Supported_Plate_Type.LT_ARMPOL_ZONGDUI:// 武警总队车牌
          break;
        case Supported_Plate_Type.LT_ARMPOL2_ZONGDUI:// 双层武警总队车牌
          break;
        case Supported_Plate_Type.LI_AVIATION://民航
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_green.png"));
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        case Supported_Plate_Type.LI_ENERGY://新能源小汽车
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_new_energy.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.LONG_SIZE;
          break;
        case Supported_Plate_Type.LI_ENERGY2://大型新能源
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_yello_energy.png"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.LONG_SIZE;
          break;
        case Supported_Plate_Type.LI_EMERGENCY://应急车牌
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_white.jpg"));
          fond_color_ = ("#FF000000");
          plate_size_type_ = Supported_Plate_SIZE_Type.LONG_SIZE;
          break;
        case Supported_Plate_Type.LI_CONSULATE://领事馆
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_black.png"));
          fond_color_ = ("#FFFFFFFF");
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;
          break;
        default:
          plate_bitmap_ = Set_attribute(Path.Combine(in_path_, "civil_blue.png"));//设置底图
          fond_color_ = ("#FFFFFFFF");//字体颜色
          plate_size_type_ = Supported_Plate_SIZE_Type.NORMAL_SIZE;//设置尺寸类型
          break;
      }
      //resize
      if (plate_bitmap_ == null) return;
      if(plate_size_type_ == Supported_Plate_SIZE_Type.NORMAL_SIZE){
        plate_bitmap_ = plate_bitmap_.Resize(new SKImageInfo(440, 140), SKFilterQuality.High);//resize
      }
      else if(plate_size_type_ == Supported_Plate_SIZE_Type.LONG_SIZE)
      {
        plate_bitmap_ = plate_bitmap_.Resize(new SKImageInfo(480, 140), SKFilterQuality.High);//resize
      }
      else
      {
        plate_bitmap_ = plate_bitmap_.Resize(new SKImageInfo(440, 220), SKFilterQuality.High);//resize
      }
    }
    //绘制车牌分隔圆点
    protected void Paint_char_dot(float dot_pos_x, float dot_pos_y, float dot_radius, string? fond_color)
    {
      if (fond_color == null) return;
      //创建SKCanvas对象
      using (SKCanvas canvas = new SKCanvas(plate_bitmap_))
      {
        using (SKPaint paint = new SKPaint())//颜色设置
        {
          paint.Color = new SKColor(uint.Parse(fond_color.Substring(1, fond_color.Length - 1)
                                      , System.Globalization.NumberStyles.AllowHexSpecifier));
          canvas.DrawCircle(dot_pos_x, dot_pos_y, dot_radius, paint);

          // canvas.DrawBitmap(resourceBitmap, source_char_pos, dest_char_pos, paint);
          // Save_bitmap(plate_bitmap_);
        }
      }
    }
    //根据字符找出在图中的位置
    protected SKRect read_char_pos(char license_c, bool is_down = true, bool is_sigle_char_img = false)
    {
      int find_idx;
      if (plate_type_ == Supported_Plate_Type.LI_ENERGY || plate_type_ == Supported_Plate_Type.LI_ENERGY2)
        find_idx = char_db_tab.LastIndexOf(license_c);
      else
        find_idx = char_db_tab.IndexOf(license_c);
      int img_size_x = 224*4;//图像整体尺寸
      int word_size_x = 28 * 4;//单个字符尺寸
      int word_size_y = 50 * 4;
      if(!is_sigle_char_img)//是否只有一张字符图
      {
        if (plate_type_ == Supported_Plate_Type.LI_ENERGY || plate_type_ == Supported_Plate_Type.LI_ENERGY2)//新能源车牌
        {
          img_size_x = 344;//图像整体尺寸
          word_size_x = 43;//单个字符尺寸
          word_size_y = 90;
        }
        else if (plate_size_type_ == Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE)//双层车牌
        {
          if (is_down)
          {
            img_size_x = 880;//图像整体尺寸
            word_size_x = 110;//单个字符尺寸
            word_size_y = 185;
          }
          else
          {
            img_size_x = 1144;//图像整体尺寸
            word_size_x = 143;//单个字符尺寸
            word_size_y = 110;
          }
        }
      }


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
    //字符中心以及大小推算字符四个点
    protected void Calculate_SKRect(float ceter_x, float ceter_y, float word_width, float word_height, ref SKRect rect)
    {
      rect.Left = ceter_x - word_width / 2;
      rect.Right = ceter_x + word_width / 2;
      rect.Top = ceter_y - word_height / 2;
      rect.Bottom = ceter_y + word_height / 2;
    }
    //绘制车牌号单个字符
    protected void Paint_char_image(SKRect source_char_pos, SKRect dest_char_pos, string? fond_color, bool is_dowm = true, bool is_sigle_char_img = false)
    {
      if (In_path_ == null || fond_color == null) return;
      string input_p = "civil_db_112x200.jpg";
      if(!is_sigle_char_img)
      {
        if (plate_type_ == Supported_Plate_Type.LI_ENERGY || plate_type_ == Supported_Plate_Type.LI_ENERGY2)//新能源车牌
        {
          input_p = "civil_db_112x200_green.jpg";
        }
        else if (plate_size_type_ == Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE)//双层车牌
        {
          if (is_dowm)
            input_p = "civil_db_112x200_220_down.jpg";
          else
          {
            input_p = "civil_db_112x200_220_up.jpg";
            Console.WriteLine("{0}", plate_size_type_);
          }
        }
      }

      input_p = Path.Combine(In_path_, input_p);
      using (var input = File.OpenRead(input_p))//读取图片文件
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
            }
          }
        }
      }
    }
    //绘制车牌
    protected void Paint_license_image()
    {
      if(license_str_ == null || plate_bitmap_ == null) return;
      if (fond_color_ == null) { fond_color_ = "#FFFFFFFF"; }
      string? dot_color = fond_color_;//车牌分隔点颜色
      //对武警
      if (license_str_[0] == 'W' && license_str_[1] == 'J')
      {
        license_str_ = license_str_.Remove(0,2).Insert(0, '武'.ToString());
      }
      //Console.WriteLine("str_license_： {0}, plate_type_： {1}", license_str_, plate_type_);
      //计算每个字符在模拟车牌上的位置
      int standH = 140;//车牌高度
      int standW = 440;//车牌宽度
      int tab_start = 38;//首字符位置
      int espase_width = 12;//字符间间隔
      float dot_radius = 5;//分隔点半径
      float word_width = 45;//字符宽度
      float word_height = 90;//字符高度
      float dot_pos_x = 0;
      float dot_pos_y = 0;

      //普通车牌
      float[] wtab_x = new float[8] {//车牌字符中心位置
          tab_start,
          tab_start+(word_width+espase_width),
          tab_start+(word_width+espase_width)*2+dot_radius*2+espase_width,
          tab_start+(word_width+espase_width)*3+dot_radius*2+espase_width,
          tab_start+(word_width+espase_width)*4+dot_radius*2+espase_width,
          tab_start+(word_width+espase_width)*5+dot_radius*2+espase_width,
          tab_start+(word_width+espase_width)*6+dot_radius*2+espase_width,
          tab_start+(word_width+espase_width)*7+dot_radius*2+espase_width};
      float[] wtab_y = new float[8];
      SKRect[] dest_char_pos = new SKRect[8];//车牌字符位置

      switch (plate_size_type_)
      {
        case Supported_Plate_SIZE_Type.LONG_SIZE://八位车牌
          //车牌字符
          int word_width_energy = 43;
          if(plate_type_ == Supported_Plate_Type.LI_EMERGENCY)
          {
            espase_width = 12;
            float espase_width_dot = 8.5F;
            dot_radius = 7F;
            wtab_x[1] = wtab_x[0] + (word_width + word_width_energy) / 2 + espase_width_dot * 2 + dot_radius * 2;
            wtab_x[2] = wtab_x[1] + word_width_energy + espase_width;
            //车牌分隔点
            dot_pos_x = (wtab_x[0] + wtab_x[1]) / 2;
            dot_pos_y = standH / 2;
            dot_color = fond_color_;
          }
          else
          {
            espase_width = 9;
            dot_radius = 15.5F;
            wtab_x[1] = wtab_x[0] + ((word_width + word_width_energy) / 2 + espase_width);
            wtab_x[2] = wtab_x[1] + word_width_energy + espase_width + dot_radius * 2 + espase_width;
            dot_color = null;
          }
          wtab_x[3] = wtab_x[2] + word_width_energy + espase_width;
          wtab_x[4] = wtab_x[3] + word_width_energy + espase_width;
          wtab_x[5] = wtab_x[4] + word_width_energy + espase_width;
          wtab_x[6] = wtab_x[5] + word_width_energy + espase_width;
          wtab_x[7] = wtab_x[6] + word_width_energy + espase_width;
          Calculate_SKRect(wtab_x[0], standH / 2, word_width, word_height, ref dest_char_pos[0]);
          for (int i = 1; i < 8; i++)
          {
            Calculate_SKRect(wtab_x[i], standH / 2, word_width_energy, word_height, ref dest_char_pos[i]);
          }
          break;
        case Supported_Plate_SIZE_Type.DOUBLE_DECK_SIZE://双层车牌
          word_width = 65;
          word_height = 110;
          tab_start = 60;
          espase_width = 15;
          float tab_start_h = 145;
          float word_width_double = 80;
          float word_height_double = 60;
          float tab_start_double = 150;
          float espase_width_double = 25;
          float tab_start_h_double = 45;
          //中心点位置
          wtab_x[0] = tab_start_double;
          wtab_x[1] = wtab_x[0] + word_width_double + espase_width_double + dot_radius * 2 + espase_width_double;
          wtab_x[2] = tab_start;
          wtab_x[3] = wtab_x[2] + word_width + espase_width;
          wtab_x[4] = wtab_x[3] + word_width + espase_width;
          wtab_x[5] = wtab_x[4] + word_width + espase_width;
          wtab_x[6] = wtab_x[5] + word_width + espase_width;
          wtab_x[7] = wtab_x[1] + word_width_double + espase_width_double;//用于绘制农用车
          //rect
          for (int i = 0; i < 8; i++)
          {
            if(i<=1 || i == 7)
              Calculate_SKRect(wtab_x[i], tab_start_h_double, word_width_double, word_height_double, ref dest_char_pos[i]);
            else
              Calculate_SKRect(wtab_x[i], tab_start_h, word_width, word_height, ref dest_char_pos[i]);
          }
          //计算车牌分隔点在模拟车牌上的位置
          dot_pos_x = (wtab_x[0] + wtab_x[1]) / 2;
          dot_pos_y = tab_start_h_double;
          break;
        default://普通车牌
          for(int i = 0;i<8;i++)
          {
            Calculate_SKRect(wtab_x[i], standH / 2, word_width, word_height, ref dest_char_pos[i]);
          }
          //计算车牌分隔点在模拟车牌上的位置
          dot_pos_x = (wtab_x[1] + wtab_x[2]) / 2;
          dot_pos_y = standH / 2;
          break;
          
      }

      //车牌种类特殊性调整
      string[] paint_color = new string[8];//字体颜色
      for(int i = 0;i<8; i++)
      {
        paint_color[i] = fond_color_;
      }
      switch (plate_type_)
      {
        case Supported_Plate_Type.LT_POLICE://单排警车小汽车
          wtab_x[1] += (espase_width + dot_radius * 2);
          Calculate_SKRect(wtab_x[1], standH / 2, word_width, word_height, ref dest_char_pos[1]);
          dot_pos_x = (wtab_x[0] + wtab_x[1]) / 2;
          paint_color[6] = "#FFFF0000";
          dot_radius = 7;
          break;
        case Supported_Plate_Type.LT_ARMPOL://单排武警小汽车
          paint_color[0] = "#FFFF0000";
          paint_color[1] = "#FFFF0000";
          dot_color = "#FFFF0000";
          break;
        case Supported_Plate_Type.LT_ARMY://单排军车牌
          paint_color[0] = "#FFFF0000";
          paint_color[1] = "#FFFF0000";
          dot_color = "#FFFF0000";
          break;
        case Supported_Plate_Type.LT_INDIVI://个性化车牌
          break;
        case Supported_Plate_Type.LT_ARMY2://双排军车牌
          paint_color[0] = "#FFFF0000";
          paint_color[1] = "#FFFF0000";
          dot_color = null;
          break;
        case Supported_Plate_Type.LT_EMBASSY://使馆车牌
          wtab_x[2] -= (espase_width + dot_radius * 2);
          Calculate_SKRect(wtab_x[2], standH / 2, word_width, word_height, ref dest_char_pos[2]);
          dot_pos_x = (wtab_x[2] + wtab_x[3]) / 2;
          break;
        case Supported_Plate_Type.LT_TRACTOR://农用车牌
          char license_c = license_str_[2];
          license_str_ = license_str_.Remove(2, 1);
          license_str_ += license_c;
          //居中
          float offset = (wtab_x[0] + wtab_x[7] - standW) / 2;
          dest_char_pos[0].Left -= offset;
          dest_char_pos[0].Right -= offset;
          dest_char_pos[1].Left -= offset;
          dest_char_pos[1].Right -= offset;
          dest_char_pos[7].Left -= offset;
          dest_char_pos[7].Right -= offset;
          dot_color = null;
          break;
        case Supported_Plate_Type.LT_ARMPOL2://双层武警车牌
          paint_color[0] = "#FFFF0000";
          paint_color[1] = "#FFFF0000";
          dot_color = "#FFFF0000";
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
          paint_color[1] = "#FFFF0000";
          paint_color[6] = "#FFFF0000";
          paint_color[7] = "#FFFF0000";
          break;
        case Supported_Plate_Type.LI_CONSULATE://领事馆
          wtab_x[2] -= (espase_width + dot_radius*2);
          Calculate_SKRect(wtab_x[2], standH / 2, word_width, word_height, ref dest_char_pos[2]);
          wtab_x[3] -= (espase_width + dot_radius * 2);
          Calculate_SKRect(wtab_x[3], standH / 2, word_width, word_height, ref dest_char_pos[3]);
          dot_pos_x = (wtab_x[3] + wtab_x[4]) / 2;
          break;
        default:
          break;
      }
      //绘制车牌分隔圆点
      Paint_char_dot(dot_pos_x, dot_pos_y, dot_radius, dot_color);
      int temp_index = 0;
      bool is_down = true;
      foreach (char license_c in license_str_) {
        if (temp_index == 0 || temp_index == 1 || temp_index == 7)
          is_down = false;
        else
          is_down = true;
        //得到字符在字符图中位置
        SKRect source_char_pos = read_char_pos(license_c, is_down, is_is_sigle_char_img_);
        //计算字符在模拟车牌图中的位置
        //根据位置绘制车牌字符
        Paint_char_image(source_char_pos, dest_char_pos[temp_index], paint_color[temp_index], is_down, is_is_sigle_char_img_);
        ++temp_index;
      }
    }
    protected void Save_bitmap(SKBitmap? plate_bitmap, string out_path)//保存图片
    {
      if (plate_bitmap == null || out_path_ == null) return;
        //plate_type_str
      if (System.IO.File.Exists(Path.GetFullPath(out_path)))//文件存在就删除
      {
        File.Delete(Path.GetFullPath(out_path));
      }
      using (var writeStream = System.IO.File.OpenWrite(out_path))
      {
        plate_bitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(writeStream);
      }
    }
    public void paint_plate_image()//合成模拟图
    {
      Console.WriteLine("str_license_： {0}, plate_type_： {1}", license_str_, plate_type_);
      Paint_bk_image();//设置车牌属性
      Paint_license_image();//设置车牌字符图
      string plate_type_str = Enum.GetName(typeof(Supported_Plate_Type), plate_type_) + ".png";
      string out_path = Path.Combine(Out_path_, plate_type_str);
      Save_bitmap(plate_bitmap_, out_path);//保存车牌模拟图
    }

    public void add_char_image(string source_img_p, string resource_bitmap_p, int pos)//添加寻字图
    {
      int img_size_x = 896;//图像整体尺寸
      int img_size_y = 2200;
      int word_size_x = 112;//单个字符尺寸
      int word_size_y = 200;
      if (in_path_ == null || out_path_ == null) { return; }
      SKBitmap? source_img_bitmap = Set_attribute(source_img_p);
      SKBitmap? resource_bitmap = Set_attribute(resource_bitmap_p);
      if (source_img_bitmap == null || resource_bitmap == null) return;
      int char_db_row_elem_num = img_size_x / word_size_x;//单行字符数量
      int x_idx = pos % char_db_row_elem_num;//字符列数
      int y_idx = pos / char_db_row_elem_num;//字符行数
      //字符所在位置
      float dest_char_pos_left = x_idx * word_size_x;
      float dest_char_pos_right = (x_idx + 1) * word_size_x;
      float dest_char_pos_top = y_idx * word_size_y;
      float dest_char_pos_bottom = (y_idx + 1) * word_size_y;
      SKRect dest_char_pos = new SKRect(dest_char_pos_left, dest_char_pos_top, dest_char_pos_right, dest_char_pos_bottom);
      SKRect source_char_pos = new SKRect(0, 0, resource_bitmap.Width, resource_bitmap.Height);
      using (SKCanvas canvas = new SKCanvas(source_img_bitmap))
      {
        using (SKPaint paint = new SKPaint())//颜色设置
        {
          paint.ColorFilter =
          SKColorFilter.CreateColorMatrix(new float[]
          {
                -1f, 0, 0, 1f, 0,
                -1f, 0, 0, 1f, 0,
                -1f, 0, 0, 1f, 0,
                0, 0, 0, 1f, 0
          });
          canvas.DrawBitmap(resource_bitmap, source_char_pos, dest_char_pos, paint);
        }
      }   
      Save_bitmap(source_img_bitmap, source_img_p);
    }

    public string? License_str_
    {
      get
      {
        return license_str_;
      }
      set
      {
        if (value == null) return;
        license_str_ = value;
      }
    }
    public Supported_Plate_Type Plate_type_
    {
      get
      {
        return plate_type_;
      }
      set
      {
        if (!Enum.IsDefined(typeof(Supported_Plate_Type), value)) return;
        if (value == Supported_Plate_Type.LT_UNKNOWN || value == Supported_Plate_Type.LT_INDIVI ||
         value == Supported_Plate_Type.LT_ARMPOL_ZONGDUI || value == Supported_Plate_Type.LT_ARMPOL2_ZONGDUI)
          return;
        plate_type_ = value;
      }
    }
    public string? Out_path_
    {
      get
      {
        return out_path_;
      }
      set
      {
        if (value == null) return;
        out_path_ = value;
      }
    }

    public string? In_path_
    {
      get
      {
        return in_path_;
      }
      set
      {
        if (value == null) return;
        in_path_ = value;
      }
    }

    public SKBitmap? Plate_bitmap_
    {
      get
      {
        return plate_bitmap_;
      }
    }

    public bool Is_is_sigle_char_img_
    {
      get
      {
        return Is_is_sigle_char_img_;
      }
      set
      {
        is_is_sigle_char_img_ = value;
      }
    }
    //成员变量
    private string? license_str_; //车牌号
    private Supported_Plate_Type plate_type_;//车牌种类
    Supported_Plate_SIZE_Type plate_size_type_;//车牌尺寸类别
    private SKBitmap? plate_bitmap_;//车牌模拟图
    private string? fond_color_;//车牌字体颜色
    private string? in_path_;//图片保存路径
    private string? out_path_;//输出路径
    private bool is_is_sigle_char_img_;
  }
}