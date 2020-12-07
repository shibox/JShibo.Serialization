# JShibo.Serialization
注意：该框架现在不是功能还未完成，仅供研究，尽请期待

the world fastest json Serialize Framework for .Net Core
it support json,csv,tsv,binary,xml and so on
超高性能序列化库，可能是世界上最快的序列化框架，为性能而生

### Easy to use 简单使用
```C#

public class Book
{
    public int Id { get; set; } = 123;
    public int Name { get; set; } = "test";
}

public class Demo
{
    public static void Main()
    {
        var json = ShiboSerializer.Serialize(new Book());
        var obj = ShiboSerializer.Deserialize<Book>(json);
    }
}
```


