using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Converters;
using WMS.Models.Dtos.LayoutDtos;
using Xunit;

namespace WMS.UnitTests.ConventersTests
{
    public class PositionConverterTests
    {

        [Theory]
        [InlineData("0.3.0", 0, 3, 0)]
        [InlineData("4.3.5", 4, 3, 5)]
        [InlineData("7.3.6", 7, 3, 6)]
        public void ConvertToStruct_WithValidPositionString_ReturnsCoordinates(string position, int x, int y, int z)
        {
            //arrange
            //act

            var result = position.ConvertToStruct();

            //assert
            result.X.Should().Be(x);
            result.Y.Should().Be(y);
            result.Z.Should().Be(z);

        }
    }
}

//public static Coordinates ConvertToStruct(this string position)
//{
//    if (position is null)
//        throw new NotFoundException("position Not Found");

//    var splitted = position.Split('.');

//    int x = 0;
//    int y = 0;
//    int z = 0;

//    bool successfullyParsed = true;

//    successfullyParsed = successfullyParsed & int.TryParse(splitted[0], out x);
//    successfullyParsed = successfullyParsed & int.TryParse(splitted[1], out y);
//    successfullyParsed = successfullyParsed & int.TryParse(splitted[2], out z);

//    if (!successfullyParsed)
//        throw new ConverterException("Cannot convert position");

//    var result = new Coordinates(x, y, z);

//    return result;