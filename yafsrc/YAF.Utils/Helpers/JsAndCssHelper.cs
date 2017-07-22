/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2017 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Utils.Helpers
{
    using System;

    using YAF.Utils.Helpers.MinifyUtils;

    /// <summary>
  /// The js and css helper.
  /// </summary>
  public static class JsAndCssHelper
  {
    /// <summary>
    /// Compresses JavaScript
    /// </summary>
    /// <param name="javaScript">
    /// The Uncompressed Input JS
    /// </param>
    /// <returns>
    /// The compressed java script.
    /// </returns>
    public static string CompressJavaScript(string javaScript)
    {
        try
        {
           return JSMinify.Minify(javaScript);
        }
        catch (Exception)
        {
            return javaScript;
        }
    }

    /// <summary>
    /// Compresses CSS
    /// </summary>
    /// <param name="css">
    /// The Uncompressd Input CSS
    /// </param>
    /// <returns>
    /// The compressed css output.
    /// </returns>
    public static string CompressCss(string css)
    {
        try
        {
            return JSMinify.Minify(css);
        }
        catch (Exception)
        {
            return css;
        }
    }
  }
}