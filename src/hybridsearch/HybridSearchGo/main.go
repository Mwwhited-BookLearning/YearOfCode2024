package main

import (
	"github.com/gorilla/mux"
	"hybrid-search/webapi/controllers/fileController"
	"hybrid-search/webapi/providers/blobs"
	"hybrid-search/webapi/providers/embed"
	"hybrid-search/webapi/providers/hybrid"
	"hybrid-search/webapi/providers/lexical"
	"hybrid-search/webapi/providers/semantic"
	"net/http"
)

func main() {
	router := mux.NewRouter()

	semantic := semantic.Create()
	lexical := lexical.Create()
	hybrid := hybrid.Create(lexical, semantic)

	embed := embed.Create()

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

	http.Handle("/", router)
	http.ListenAndServe("0.0.0.0:3080", nil)
}
