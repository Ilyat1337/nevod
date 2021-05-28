﻿//--------------------------------------------------------------------------------------------------
// Copyright © Nezaboodka™ Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Nezaboodka.Nevod
{
    public class OutsideSyntax : Syntax
    {
        public Syntax Body { get; }
        public new Syntax Exception { get; }

        internal OutsideSyntax(Syntax body, Syntax exception)
        {
            Body = body;
            Exception = exception;
        }

        internal OutsideSyntax Update(Syntax body, Syntax exception)
        {
            OutsideSyntax result = this;
            if (body != Body || exception != Exception)
                result = Outside(body, exception);
            return result;
        }

        protected internal override Syntax Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitOutside(this);
        }
    }

    public partial class Syntax
    {
        public static OutsideSyntax Outside(Syntax body, Syntax exception)
        {
            var result = new OutsideSyntax(body, exception);
            return result;
        }
    }
}
