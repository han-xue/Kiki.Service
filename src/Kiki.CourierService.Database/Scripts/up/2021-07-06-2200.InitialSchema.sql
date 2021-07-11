CREATE TABLE [dbo].[Offer]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [OfferCode] VARCHAR(100) NOT NULL,
    [DiscountRate] DECIMAL(5,2) NOT NULL,
    [IsActive] BIT NOT NULL,
    [DistanceRangeFrom] DECIMAL(8,2)  NOT NULL,
    [DistanceRangeTo] DECIMAL(8,2)  NOT NULL,
    [WeightRangeFrom] DECIMAL(8,2)  NOT NULL,
    [WeightRangeTo] DECIMAL(8,2)  NOT NULL
)