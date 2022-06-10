using System;
using FluentAssertions;
using Smart.FA.Catalog.Core.Extensions;
using Xunit;

namespace Smart.FA.Catalog.UnitTests.Extensions;
public class TypeExtensionsTest
{
    [Fact]
    public void DerivesFromGenericType_WhenAClassImplementsAGenericInterface_ShouldReturnTrue()
    {
        var @class = new ClassThatImplementsAGenericInterface();

        var derives = @class.GetType().DerivesFromGenericType(typeof(IInterface<>));

        derives.Should().BeTrue();
    }

    [Fact]
    public void DerivesFromGenericType_WhenAClassImplementsANonGenericInterface_ShouldReturnTrue()
    {
        var @class = new ClassThatImplementsAGenericInterface();

        var derives = @class.GetType().DerivesFromGenericType(typeof(IInterface));

        derives.Should().BeFalse();
    }

    [Fact]
    public void DerivesFromGenericType_WhenAClassInheritsFromAGenericClass_ShouldReturnTrue()
    {
        // Arrange
        var @class = new GenericClass();

        // Act
        var derives = @class.GetType().DerivesFromGenericType(typeof(GenericClass<>));

        // Assert
        derives.Should().BeTrue();
    }

    [Fact]
    public void DerivesFromGenericType_WhenAClassDoesNotInheritsFromAGenericClass_ShouldReturnFalse()
    {
        // Arrange
        var @class = new NonGenericClass();

        // Act
        var derives = @class.GetType().DerivesFromGenericType(typeof(object));

        // Assert
        derives.Should().BeFalse();
    }


    public class ClassThatImplementsAGenericInterface : IInterface<int> { }

    public class GenericClass : GenericClass<int> { }

    public class GenericClass<T> {}

    public class NonGenericClass : Exception { }

    private interface IInterface { }

    private interface IInterface<T> : IInterface { }

}
