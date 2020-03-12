# AspnetcoreKong
aspnetcore微服务自动注册到kong的解决方案


### 如何使用
* 将kongsettings.json加载到配置，或者将其内容复制到应用配置文件
* 在Startup 添加services.AddKong();和 app.UseKong();

### Nuget安装
<PackageReference Include="Kong.Aspnetcore" Version="1.0.0" />
