Create function fn_Usuarios() 

returns table 
as 
return 
	select u.CodigoUser, r.Descripcion AS Rol, u.NombreUser, (TRIM(u.Nombres) + TRIM(u.Apellidos)) AS NombreCompleto, u.CorreoElectronico, u.Activo, u.FechaRegistrado
	from Usuarios u 
	inner join sys_roles r on r.CodigoRol = u.CodigoRol 


	