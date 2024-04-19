package main

import (
	"github.com/gin-gonic/gin"

	"hybrid-search/webapi/controllers/fileController"
	docs "hybrid-search/webapi/docs"
	"hybrid-search/webapi/providers/blobs"
	"hybrid-search/webapi/providers/embed"
	"hybrid-search/webapi/providers/hybrid"
	"hybrid-search/webapi/providers/lexical"
	"hybrid-search/webapi/providers/semantic"

	swaggerfiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

func main() {
	docs.SwaggerInfo.BasePath = "/api"

	router := gin.Default()

	embed := embed.Create(embed.CreateOptions())

	semantic := semantic.Create()

	lexical := lexical.Create(*lexical.CreateClientFactory(lexical.CreateOptions()).Create())

	hybrid := hybrid.Create(lexical, semantic)

	blobClient := blobs.CreateClientFactory().Create(blobs.CreateOptions())
	blobDocs := blobs.CreateBlobProviderFactory().Create(blobClient, "docs")
	blobSummary := blobs.CreateBlobProviderFactory().Create(blobClient, "summary")

	fileController.Build(
		router,
		hybrid,
		lexical,
		semantic,
		embed,
		blobDocs,
		blobSummary)

	router.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerfiles.Handler))
	router.Run(":3080")
}
