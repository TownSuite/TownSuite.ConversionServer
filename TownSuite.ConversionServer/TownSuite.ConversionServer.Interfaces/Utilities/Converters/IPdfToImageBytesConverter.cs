﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TownSuite.ConversionServer.Common.Models;
using TownSuite.ConversionServer.Interfaces.Common.Validation;

namespace TownSuite.ConversionServer.Interfaces.Utilities.Converters
{
    public interface IPdfToImageBytesConverter
    {
        Task<IEnumerable<byte[]>> Convert(byte[] pdf, CancellationToken cancellationToken = default);
        Task<StreamFileResults> Convert(IUploadedStreamHandler streamHandler, CancellationToken cancellationToken = default);
    }
}
