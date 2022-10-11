Create procedure [dbo].[AddNewCliDetails]  
(  
   @Login varchar (50),  
   @Haslo varchar (50)
)  
as  
begin  
   Insert into Clients values(@Login,@Haslo)  
End  