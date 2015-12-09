using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JNinject.Core.UnitTest
{
    [TestFixture]
    public class ContainerBuilderTest
    {
        [Test]
        public void ResolveWithDelegateTest()
        {
            IJContainerBuilder builder = new JContainerBuilder();

            builder.Register("Nick")
                .Named("StudentName");

            builder.Register("台北市大安區")
                .Named("AddressDic");

            builder.Register("復興南部222號")
                .Named("AddressStreet");

            builder.RegisterType(ctx => new Student(ctx.ResolveName<string>("StudentName"), 10, ctx.Resolve<IAddress>()))
                .As<Student>();

            builder.RegisterType<Address>(ctx => new Address(ctx.ResolveName<string>("AddressDic"), 
                ctx.ResolveName<string>("AddressStreet")))
                .As<IAddress>();

            var container = builder.Build();
            
            var student = container.Resolve<Student>();

            Assert.AreEqual("Nick", student.Name);
            Assert.AreEqual(10, student.Age);
            Assert.AreEqual("台北市大安區", student.Address.Dict);
            Assert.AreEqual("復興南部222號", student.Address.Street);
        }


        [Test]
        public void ResolveTest()
        {
            IJContainerBuilder builder = new JContainerBuilder();

            builder.RegisterType<Outter>()
                .As<IOutter>();

            builder.RegisterType<Inner>()
                .As<IInner>();

            var container = builder.Build();
            
            var outter = container.Resolve<IOutter>();

            Assert.IsNotNull(outter);
            Assert.IsNotNull(outter.Inner);
        }

        [Test]
        public void SingltonTest()
        {
            IJContainerBuilder builder = new JContainerBuilder();

            builder.RegisterType<Outter>()
                .As<IOutter>()
                .SingleInstance();

            builder.RegisterType<Inner>()
                .As<IInner>()
                .SingleInstance();

            builder.Register("台北市大安區")
                .Named("AddressDic");

            builder.Register("復興南部222號")
                .Named("AddressStreet");

            builder.RegisterType(ctx => new Address(ctx.ResolveName<string>("AddressDic"),
                ctx.ResolveName<string>("AddressStreet")))
                .As<IAddress>()
                .SingleInstance();

            var container = builder.Build();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var outter = container.Resolve<IOutter>();
            var inner = container.Resolve<IInner>();
            var outter2 = container.Resolve<IOutter>();
            var address = container.Resolve<IAddress>();
            var address2 = container.Resolve<IAddress>();
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);

            Assert.AreSame(outter, outter2);
            Assert.AreSame(outter.Inner, inner);
            Assert.AreSame(address, address2);
        }

        [Test]
        public void GenerateLifeMixSingltonTest()
        {
            IJContainerBuilder builder = new JContainerBuilder();

            builder.RegisterType<Outter>()
                .As<IOutter>();

            builder.RegisterType<Inner>()
                .As<IInner>()
                .SingleInstance();

            var container = builder.Build();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var outter = container.Resolve<IOutter>();
            var inner = container.Resolve<IInner>();
            var outter2 = container.Resolve<IOutter>();
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);

            Assert.AreNotSame(outter, outter2);
            Assert.AreSame(outter.Inner, inner);
        }
    }

    public class Student
    {
        public Student(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public Student(string name, int age, IAddress address)
        {
            Name = name;
            Age = age;
            Address = address;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public IAddress Address { get; set; }

    }

    public interface IAddress
    {
        string Dict { get; }
        string Street { get; }
    }

    public class Address : IAddress
    {
        public Address()
        { }

        public Address(string dic, string street)
        {
            Dict = dic;
            Street = street;
        }

        public string Dict { get; set; }
        public string Street { get; set; }
    }

    public interface IOutter { IInner Inner { get; } }

    public interface IInner {}

    public class Outter : IOutter
    {
        public IInner Inner { get; set; }

        public void TTTDDD() { }

        public Outter(IInner inner)
        {
            this.Inner = inner;
        }
    }

    public class Inner : IInner
    {

    }
}
