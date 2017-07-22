﻿<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="YAF.ForumPageBase" %>
<%@ Register TagPrefix="YAF" Assembly="YAF" Namespace="YAF" %>
<%@ Register TagPrefix="url" Namespace="Intelligencia.UrlRewriter" Assembly="Intelligencia.UrlRewriter" %>
<script runat="server">
</script>
<!doctype html>
<html lang="en">
<head id="YafHead" runat="server">
     <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="x-ua-compatible" content="ie=edge"><meta id="YafMetaScriptingLanguage" http-equiv="Content-Script-Type" runat="server"
        name="scriptlanguage" content="text/javascript" />
    <meta id="YafMetaStyles" http-equiv="Content-Style-Type" runat="server" name="styles"
        content="text/css" />
    <meta id="YafMetaDescription" runat="server" name="description" content="Yet Another Forum.NET -- A bulletin board system written in ASP.NET" />
    <meta id="YafMetaKeywords" runat="server" name="keywords" content="Yet Another Forum.net, Forum, ASP.NET, BB, Bulletin Board, opensource" />
    <title></title>
</head>
<body id="YafBody" runat="server" style="margin: 0; padding: 5px">
    <asp:HyperLink runat="server" id="BannerLink" >
        <img src="~/forumlogo.jpg" runat="server" alt="logo" style="border: 0;" id="imgBanner" />
    </asp:HyperLink>
    <br />
    <url:Form id="form1" runat="server" enctype="multipart/form-data">
    <YAF:Forum runat="server" ID="forum" BoardID="1">
    </YAF:Forum>
    </url:Form>
</body>
</html>
