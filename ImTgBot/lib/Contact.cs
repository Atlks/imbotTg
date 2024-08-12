using System.ComponentModel;

namespace prjx
{
    /// <summary>
    /// 城市
    /// </summary>
    public class City
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 城市群ID
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 地址关键词
        /// </summary>
        public string CityKeywords { get; set; } = null!;
        /// <summary>
        /// 城市/园区
        /// </summary>
        public HashSet<Address> Address { get; set; } = null!;
    }
    /// <summary>
    /// 城市/园区
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 城市/园区名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 园区城市群ID
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 地址关键词
        /// </summary>
        public string CityKeywords { get; set; } = null!;
        /// <summary>
        /// 商家集合
        /// </summary>
        public HashSet<Merchant> Merchant { get; set; } = [];
    }

    public class Merchant
    {
        public int ord { get; set; } = 0;
        public string Guid { get; set; } = null!;
        /// <summary>
        /// 分类枚举
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 关键词
        /// </summary>
        public string KeywordString { get; set; } = null!;
        /// <summary>
        /// 搜索展现量
        /// </summary>
        public int Searchs { get; set; } = 0;
        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; } = 0;
        /// <summary>
        /// Google位置
        /// </summary>
        public string? GoogleMapLocator { get; set; }
        /// <summary>
        /// 开始营业时间
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 截止营业时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 纸飞机联系方式
        /// </summary>
        public List<string> Telegram { get; set; } = [];
        /// <summary>
        /// 纸飞机群链接
        /// </summary>
        public string? TelegramGroup { get; set; }
        /// <summary>
        /// WhatsApp
        /// </summary>
        public List<string> WhatsApp { get; set; } = [];
        /// <summary>
        /// Line
        /// </summary>
        public List<string> Line { get; set; } = [];
        /// <summary>
        /// Signal
        /// </summary>
        public List<string> Signal { get; set; } = [];
        /// <summary>
        /// 微信
        /// </summary>
        public List<string> WeiXin { get; set; } = [];
        /// <summary>
        /// 电话
        /// </summary>
        public List<string> Tel { get; set; } = [];
        /// <summary>
        /// 商家菜单
        /// </summary>
        public string? Menu { get; set; }
        /// <summary>
        /// 促销活动
        /// </summary>
        public string? Promotion { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public Dictionary<long, string> Comments { get; set; } = [];

        /// <summary>
        /// 评分
        /// </summary>
        public Dictionary<long, int> Scores { get; set; } = [];
    }

    public enum Category
    {
        /// <summary>
        /// 美食
        /// </summary>
        [Description("商家 Merchant ShangJia 菜单 前台 客服 接单员 点餐 菜單 Menu Shop 外卖 店 餐馆 餐饮 餐厅 菜馆 饭馆 饭店 菜馆 酒楼 酒家 饭庄 馆子 食肆 食店 快餐 店铺 商业街 门店 店家 排挡 店面 美食街 业主 店主 预定 聚餐 食府 餐坊 餐社 饮坊 餐居 食居 饮舍 美馔 饭舍 餐寓 餐亭 饮寓")]
        Food = 0,

        /// <summary>
        /// 饮品店
        /// </summary>
        [Description("商家 Merchant ShangJia 菜单 前台 客服 接单员 点餐 餐饮 菜單 Menu Shop 外卖 店 店铺 商业街 门店 店家 店面 美食街 业主 店主 冷饮 糖水 饮品 珍珠 奶茶 咖啡 绿茶 红茶 果茶 拿铁 美式 卡布奇诺 浓缩 乌龙 玫瑰 菊花 波霸 桂圆 枸杞 茶")]
        Drinks = 1,

        /// <summary>
        /// 水果店
        /// </summary>
        [Description("商家 ShangJia 菜单 前台 客服 接单员 菜單 Menu Shop 外卖 店 水果 水果摊")]
        Fruit = 2,

        /// <summary>
        /// 手机/电脑店
        /// </summary>
        [Description("商家 ShangJia 前台 客服 店 手机 电脑  缅甸卡 泰国卡 中国卡 手机卡 平板 笔记本 台式机 一体机 游戏 轻薄 商务 设计 办公 电脑 科技 网络 宽带 最新 新款 高端 音响 耳机 鼠标 键盘 显示器 显卡 主板 固态 硬盘 内存 电源 UPS 二手 卡 手机壳 充电宝 老板 专卖 华为 Huawei 三星 Samsung 小米 Xiaomi Miui 苹果 Vivo Oppo 荣耀 Honor Iphone 一加 Oneplus 联想 戴尔 华硕 惠普 机械革命 玩家国度 神州 弘基 七彩虹 外星人 拯救者 微星")]
        Electronic = 3,

        /// <summary>
        /// 理发/美甲/美容/纹身
        /// </summary>
        [Description("商家 ShangJia 菜单 前台 客服 菜單 Menu Shop 院 店 纹身 美容 项目 护肤 医美 美甲 美瞳 护肤 美白 肉毒杆菌 肉毒 瘦脸 脱毛 整容 抽脂 祛痣 吸脂 丰胸 隆胸 疤痕 皮肤 松弛 皱纹 痤疮 痣 老人 黄褐 脂肪 秃顶 色斑 皮肤病 斑 溶脂 玻尿酸 光子 嫩肤 除皱 刷酸 法令纹 微针 皮秒 文眉 纹眉 热玛吉 热拉提 填充 微针 嗨体 剥脱 痘印 衰老 苹果肌 细纹 泪沟 痘疤 疤痕 下颚线 瘦身 减肥 暴瘦 黑头 毛孔 针 面膜 美发 理发 理头 剃头 剪发 造型 发行 理发师 发型设计 洗剪吹 洗发 洗头 吹发 烫发 卷发 染发 护发 发廊 发型师 发型造型 修剪 短发 长发 卷发 直发 中长发 剪发 理发 洗剪吹 烫发 染发 烫染套餐 发型设计 发型造型 造型师 发型师 洗发 护发 护理发膜 发膜护理 染发剂 染发剂护理 烫发剂 烫发剂护理 接发 发型修剪 直发 卷发 短发 长发 中发 层次发型 齐刘海 短发造型 长发造型 鬓角发型 时尚发型 潮流发型 韩式发型 欧美发型 男士发型 女士发型")]
        Cosmetic = 4,

        /// <summary>
        /// 兑换处
        /// </summary>
        [Description("商家 ShangJia 菜单 前台 客服 Menu Shop 店 汇率 换汇 兑换 承兑 现金 交易 汇率 VX WeiXin 货币 银行卡 转账 微信 支付宝 网银 网上 银行 在线 处 档 典当 行 USDT TRX ETH 泰铢 人民币 外汇 美元 欧元 英镑 港币 比特币 BTC")]
        Exchange = 5,

        /// <summary>
        /// 按摩/会所/KTV/酒吧
        /// </summary>
        [Description("商家 ShangJia 菜单 前台 客服 Menu Shop 店 红楼 二楼 嫖娼 妓女 妹子 鸡婆 会所 正规 按摩 掏耳 推拿 精油 全套 半套 快餐 泰式 中式 妈妈桑 会所 小姐 上门服务 水疗 桑拿 莞式 模特 技师 外围 大保健 洗脚 卡拉OK 唱歌 麦克风 预定 团建 酒水 卡拉OK 派对 伴唱 对唱 量贩 K歌 包厢 坐台 娱乐城 酒吧 喝酒")]
        Club = 6,

        /// <summary>
        /// 商店/超市/菜市场
        /// </summary>
        [Description("商家 ShangJia 菜单 前台 客服 菜單 Menu Shop 店 商场 购物 超市 商店 商超 店铺 杂货店 杂货铺 中国 本地 便利店")]
        Shop = 7,

        /// <summary>
        /// 车辆相关
        /// </summary>
        [Description("商家 ShangJia 客服 Shop 摩托 汽车 店 行 洗车 修理 保养 补胎 加油 洗车 维修 维修服务 汽车维修 发动机维修 刹车系统维修 排气系统维修 电气系统维修 空调系统维护 轮胎更换与修理 车身喷漆 底盘修理 事故车维修 保养服务 汽车保养 机油更换 滤清器更换 刹车油更换 空气滤清器更换 车辆检查与调校 火花塞更换 车灯调校 故障诊断与修复 故障诊断 OBD检测 发动机故障码 电脑诊断 灯光故障修复 刹车系统故障修复 电子控制单元（ECU）修复 定期检查 保险检查 定期维护检查 轮胎气压检查 刹车系统检查 电池检查 冷却系统检查 底盘检查 配件更换 汽车配件更换 刹车片更换 空调滤清器更换 雨刷更换 轮胎更换 蓄电池更换 灯泡更换 服务流程 服务预约 车辆接待 故障检查与报告 维修方案提供 修理与更换 车辆清洗 交车 汽车保险理赔 事故车维修与保险理赔 报案与定损 保险公司协商 维修与赔款结算 环保服务 废旧润滑油处理 环保车间 废弃零件处理")]
        Car = 8,

        /// <summary>
        /// 快递 /仓库/物流
        /// </summary>
        [Description("商家 ShangJia 前台 客服 店 快递站 物流 档口 快递员 快递服务 配送 包裹 邮寄 寄送 收件 派件 派送 快递公司 网点 跨境 速递 邮费 上门")]
        Express = 9,

        /// <summary>
        /// 医院
        /// </summary>
        [Description("医院 门诊 诊所 医生 急救 护士看诊 手术室 检查 治疗 医疗 病房 药房 中医 妇科 内科 外壳 皮肤科 眼科 牙科 耳鼻喉科 体检 看病 买药 拿药 感冒 发烧 头疼")]
        Hospital = 10,

        /// <summary>
        /// 酒店宾馆
        /// </summary>
        [Description("前台 客服 预定 酒店 客房 大堂经理 接待 套房 标间 豪华房 总统套房 入住 宾馆 旅馆 住宿 新酒店 老酒店")]
        Hotel = 11,

        /// <summary>
        /// 首饰
        /// </summary>
        [Description("首饰 珠宝 黄金 饰品")]
        Jewelry = 12,

        /// <summary>
        /// 服装/鞋包
        /// </summary>
        [Description("服装 男装 女装 男鞋 女鞋")]
        Clothing = 13,

        /// <summary>
        /// 宠物店
        /// </summary>
        [Description("宠物店")]
        Pet = 14,

        /// <summary>
        /// 物业
        /// </summary>
        [Description("物业")]
        Property = 15,
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 搜索次数
        /// </summary>
        public int Searchs { get; set; } = 0;
        /// <summary>
        /// 返回列表数
        /// </summary>
        public int Returns { get; set; } = 0;
        /// <summary>
        /// 查看联系方式次数
        /// </summary>
        public int Views { get; set; } = 0;
        /// <summary>
        /// 查看菜单数
        /// </summary>
        public int ViewMenus { get; set; } = 0;
        /// <summary>
        /// 打分次数
        /// </summary>
        public int Scores { get; set; } = 0;
        /// <summary>
        /// 评价次数
        /// </summary>
        public int Comments { get; set; } = 0;
        /// <summary>
        /// 查看联系方式时间集
        /// </summary>
        public List<DateTime> ViewTimes { get; set; } = [];
    }

    /// <summary>
    /// 查询View查看数
    /// </summary>
    public class Operas
    {
        /// <summary>
        /// 今天数
        /// </summary>
        public int Todays { get; set; } = 1;
        /// <summary>
        /// 本周数
        /// </summary>
        public int Weeks { get; set; } = 1;
        /// <summary>
        /// 本月数
        /// </summary>
        public int Months { get; set; } = 1;
        /// <summary>
        /// 一年数
        /// </summary>
        public int Years { get; set; } = 1;
        /// <summary>
        /// 全部数
        /// </summary>
        public int Totals { get; set; } = 1;
    }
}
