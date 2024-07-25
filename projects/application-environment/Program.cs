using System;
using System.Runtime.Versioning;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.Create();
        app.Run(async context =>
        {
            context.Response.Headers.Add("content-type", "text/html");
            await context.Response.WriteAsync($"Application Name: {Assembly.GetEntryAssembly().GetName().Name}<br/>");
            await context.Response.WriteAsync($"Application Base Path: {AppContext.BaseDirectory}<br/>");

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var targetFramework = entryAssembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true)[0] as TargetFrameworkAttribute;
            await context.Response.WriteAsync($"Target Framework: {targetFramework.FrameworkName}<br/>");

            await context.Response.WriteAsync($"Version: {Assembly.GetEntryAssembly().GetName().Version}<br/>");

            // Demonstrate the Equals method
            MyClass obj1 = new MyClass(1, "First");
            MyClass obj2 = new MyClass(1, "First");
            MyClass obj3 = new MyClass(2, "Second");

            await context.Response.WriteAsync($"obj1.Equals(obj2): {obj1.Equals(obj2)}<br/>");
            await context.Response.WriteAsync($"obj1.Equals(obj3): {obj1.Equals(obj3)}<br/>");
            await context.Response.WriteAsync($"obj1.GetHashCode(): {obj1.GetHashCode()}<br/>");
            await context.Response.WriteAsync($"obj2.GetHashCode(): {obj2.GetHashCode()}<br/>");
            await context.Response.WriteAsync($"obj3.GetHashCode(): {obj3.GetHashCode()}<br/>");
        });
        app.Run();
    }
}

// Intentionally only overriding Equals and not GetHashCode to introduce the bug
public class MyClass
{
    public int Id { get; }
    public string Name { get; }

    public MyClass(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        MyClass other = (MyClass)obj;
        return Id == other.Id && Name == other.Name;
    }

    // Intentionally omitting the override for GetHashCode() to introduce the bug
}
