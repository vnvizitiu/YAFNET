// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2007 Intelligencia
// Copyright 2007 Seth Yates
// 

using System.Xml;

using Intelligencia.UrlRewriter.Actions;
using Intelligencia.UrlRewriter.Utilities;

namespace Intelligencia.UrlRewriter.Parsers
{
	/// <summary>
	/// Summary description for ForbiddenActionParser.
	/// </summary>
	public sealed class ForbiddenActionParser : RewriteActionParserBase
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ForbiddenActionParser()
		{
		}

		/// <summary>
		/// The name of the action.
		/// </summary>
		public override string Name => Constants.ElementForbidden;

	    /// <summary>
		/// Whether the action allows nested actions.
		/// </summary>
		public override bool AllowsNestedActions => false;

	    /// <summary>
		/// Whether the action allows attributes.
		/// </summary>
		public override bool AllowsAttributes => false;

	    /// <summary>
		/// Parses the node.
		/// </summary>
		/// <param name="node">The node to parse.</param>
		/// <param name="config">The rewriter configuration.</param>
		/// <returns>The parsed action, or null if no action parsed.</returns>
		public override IRewriteAction Parse(XmlNode node, object config)
		{
			return new ForbiddenAction();
		}
	}
}
