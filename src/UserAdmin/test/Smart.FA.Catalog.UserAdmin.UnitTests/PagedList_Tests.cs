using System.Collections.Generic;
using FluentAssertions;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;
using Xunit;

namespace Smart.FA.Catalog.UserAdmin.UnitTests;

public class PagedListTests
{

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-1000)]
    public void CannotHaveInvalidPageNumber(int pageNumber)
    {
        var action = () => new PageItem(1, pageNumber);

        action.Should().Throw<GuardClauseException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-1000)]
    public void CannotHaveInvalidCurrentPage(int currentPage)
    {
        var action = () => new PageItem(currentPage, 1);

        action.Should().Throw<GuardClauseException>();
    }


    [Theory]
    [InlineData(9, 3, 3)]
    [InlineData(30, 12, 3)]
    [InlineData(1, 3, 1)]
    [InlineData(6, 10, 1)]
    [InlineData(0, 2, 0)]
    public void CalculateTotalPageNumber(int totalCount, int pageSize, int expectedResult)
    {
        var intArray = new List<int> {1, 2};
        var pagedList = new PagedList<int>(intArray, new PageItem(1, pageSize), totalCount );

        pagedList.TotalPages.Should().Be(expectedResult);
    }

}
