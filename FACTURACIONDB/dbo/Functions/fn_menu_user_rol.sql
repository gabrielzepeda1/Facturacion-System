CREATE FUNCTION dbo.fn_menu_user_rol() 
RETURNS TABLE 
AS 
RETURN (
		
SELECT p.cod_menu, m.etiqueta , p.consecutivo_usuario , u.usuario, u.cod_rol, r.descripcion 
FROM sys_Menu_Permisos_web AS p
INNER JOIN sys_usuario AS u ON u.consecutivo_usuario = p.consecutivo_usuario
INNER JOIN sys_menu_web AS m ON m.cod_menu = p.cod_menu
INNER JOIN sys_roles_usuarios AS r ON r.cod_rol = u.cod_rol

);	