﻿using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TownSuite.ConversionServer.Common.Bytes;
using TownSuite.ConversionServer.Common.Models.Conversions;
using TownSuite.ConversionServer.StandardServices;
using TownSuite.ConversionServer.Utilities.GhostScript;
using TownSuite.ConversionServer.Interfaces.Utilities.Converters;

namespace TownSuite.ConversionServer.Tests.GeneralTests.Utilities.GhostScript
{
    [TestFixture]
    class PdfToImageConverterTests
    {
        private DefaultDependencyInjection _dependencyInjection;

        [SetUp]
        public void InitConfiguration()
        {
            _dependencyInjection = new DefaultDependencyInjection();
        }
        
        [Test]
        public async Task ConvertTest()
        {
            var converter = _dependencyInjection.ServiceProvider.GetService<IPdfToImageBytesConverter>();
            var singlePage = System.IO.Path.Combine(GetAssetsDirectory(), "single_page_test.pdf");

            Assert.IsTrue(System.IO.File.Exists(singlePage));

            var pageBytes = await System.IO.File.ReadAllBytesAsync(singlePage);

            var res = await converter.Convert(pageBytes);

            int resCount = 0;
            foreach (var image in res)
            {
                Assert.Greater(image.Length, 0);
                resCount++;
            }
            Assert.AreEqual(1, resCount);
            
        }

        [Test]
        public async Task ConvertMultiPageTest()
        {
            var converter = _dependencyInjection.ServiceProvider.GetService<IPdfToImageBytesConverter>();
            var multiPage = System.IO.Path.Combine(GetAssetsDirectory(), "multi_page_pdf.pdf");

            Assert.IsTrue(System.IO.File.Exists(multiPage));
            var pageBytes = await System.IO.File.ReadAllBytesAsync(multiPage);

            var res = await converter.Convert(pageBytes);

            int resCount = 0;
            foreach (var image in res)
            {
                Assert.Greater(image.Length, 0, "Image cannot be zero bytes");
                resCount++;
            }
            Assert.Greater(resCount, 1, "Multipage test requires png file count greater than 1.");
        }

        private string GetAssetsDirectory()
        {
            string exeLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dir = System.IO.Path.Combine(exeLocation, "Utilities\\GhostScript\\Assets");
            Assert.IsTrue(System.IO.Directory.Exists(dir), "Testing assets directory missing. Ensure in same directory as executable.");
            return dir;
        }
    }
}
