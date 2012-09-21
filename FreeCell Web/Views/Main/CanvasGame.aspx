<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<title>Canvas Game</title>
		<script type="text/javascript" src="/Assets/Scripts/freecell.canvas.js"></script>
	</head>
	<body>
		<h1>Canvas Free Cell Game</h1>
		<div>
		
		</div>



		<script type="text/javascript" language="javascript">

			Game.init(defaultRules, defaultShuffler);
			console.dir(Game);
			
		</script>

	</body>
</html>
