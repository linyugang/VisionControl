using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*********************************************************************************************************
 * 
 *    说明：
 * 
 *    halcon图像显示控件的再次封装 
 *   20180621
 *       1.对于图像显示及其他操作全部封装到c++代码中避免c#对于hobject对象释放导致显示异常问题
 *       2.c++ cli代理对于其他自定义算法也可以按照此模式添加
 *       3.roi操作参考的是halcon官方实例
 *       4.c++代码用到了qt5
 *       5.开发环境为vs2015+halcon13+qt5.9.1
 *       
 *   作者:林玉刚   有任何疑问或建议请联系 linyugang@foxmail.com
 * 
 *********************************************************************************************************/

namespace Yoga.ImageControl
{
    public enum MessageType
    {
        MouseMessage,
        ShowOk,
        ShowNg
    }
    public class ShowMessageEventArgs
    {
        public readonly MessageType MessageType;
        public readonly string message;
        public ShowMessageEventArgs(MessageType messageType)
        {
            this.MessageType = messageType;
        }
        public ShowMessageEventArgs(string message)
        {
            this.MessageType = MessageType.MouseMessage;
            this.message = message;
        }
    }
}
