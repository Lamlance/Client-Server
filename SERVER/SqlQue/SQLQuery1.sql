DECLARE @seachID AS INT
SELECT @seachID = 1

SELECT PBook.Serial_Number, PBook.Full_Name, PBook.Phone_Number, PBook.Email,
PService.ServiceName, Comp.CompanyName,PJob.Note
FROM PhoneBook AS PBook

INNER JOIN PersonalJob AS PJob
ON PBook.Id = PJob.PhoneBookId

INNER JOIN Company AS Comp
ON Comp.Id = PJob.CompanyId

INNER JOIN ProvidedServices AS PService
ON PService.Id = PJob.ServiceId

WHERE PBook.Id = @seachID