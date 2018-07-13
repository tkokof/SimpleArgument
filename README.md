## SimpleArgument

#### Desc
 
command line parsing codes are tedious and easy to write wrong, this library is aim to ease it ~

#### Useage

eliminate condition and loop codes, we can write command line parsing codes just like this : 

```
// C#
public static void Main(string[] args)
{
    var sa = new SimpleArgument();
	sa.Add("-a0", OnArg0);
	sa.Add<int>("-a1", OnArg1);
	
	sa.Handle(args);
	// or we can parse raw args directly
	//sa.Handle("-a0 -a1 100");
}
```

details please see codes and there is a [blog(in chinese)](https://blog.csdn.net/tkokof1/article/details/81037393) about this library