### 该模块主要是UI层面以及跟UI层面渲染的强业务代码
1. 负责UI层面的业务渲染代码
2. UI层面需要的相关业务代码

```
|- Enums           UI层面枚举定义
|- Extensions      UI层面扩展方法
|- Form            Windows 窗体
|- OnRender        窗体以及Page 的渲染业务层面代码
|- OnRequest       UI层面调用Application 的业务,以及数据业务的处理逻辑等等，跟UI分离，没有任何跟UI 耦合的东西，单纯数据处理
|- Page            窗体中使用的Page页面
|- resource        资源（自图片资源等）
|- styles          自定义样式
|- Helper          客户端层面帮助工具类
|- System.config   系统级别配置文件,比如支付的识别表达式等，后续服务端可以自动调整识别或者自动添加识别规则和支付方式
|- Device.config   软件系统中的用户默认配置,启动后会迁移到我的文档的对于目录中,后续运行中的软件都是使用我的文档中的WeiPan Plugins 中的Device.config 配置文件

```