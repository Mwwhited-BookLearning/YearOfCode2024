package fileController

import (
	"github.com/gin-gonic/gin"

	"hybrid-search/webapi/controllers"
	"hybrid-search/webapi/providers"

	"log"
	"strconv"
)

type FileController struct {
	Actions []controllers.WebAction
	Router  *gin.Engine

	HybridSearchProvider   providers.HybridSearchProvider
	LexicalSearchProvider  providers.LexicalSearchProvider
	SemanticSearchProvider providers.SemanticSearchProvider

	EmbeddingProvider providers.EmbeddingProvider

	DocumentBlobProvider providers.BlobProvider
	SummaryBlobProvider  providers.BlobProvider
}

func Build(
	router *gin.Engine,
	hybrid providers.HybridSearchProvider,
	lexical providers.LexicalSearchProvider,
	semantic providers.SemanticSearchProvider,
	embedding providers.EmbeddingProvider,
	documentBlob providers.BlobProvider,
	summaryBlob providers.BlobProvider) FileController {

	service := FileController{
		Router: router,

		HybridSearchProvider:   hybrid,
		LexicalSearchProvider:  lexical,
		SemanticSearchProvider: semantic,

		EmbeddingProvider: embedding,

		DocumentBlobProvider: documentBlob,
		SummaryBlobProvider:  summaryBlob,
	}

	actions := []controllers.WebAction{
		{Pattern: "/file/download/*path", Handler: service.Download, Method: "GET"},
		{Pattern: "/file/text/*path", Handler: service.Text, Method: "GET"},
		{Pattern: "/file/html/*path", Handler: service.Html, Method: "GET"},
		{Pattern: "/file/pdf/*path", Handler: service.Pdf, Method: "GET"},
		{Pattern: "/file/summary/*path", Handler: service.Summary, Method: "GET"},

		{Pattern: "/file/list", Handler: service.List, Method: "GET"},

		{Pattern: "/file/embed", Handler: service.Embed, Method: "GET"},

		{Pattern: "/file/semantic", Handler: service.Semantic, Method: "GET"},
		{Pattern: "/file/lexical", Handler: service.Lexical, Method: "GET"},
		{Pattern: "/file/hybrid", Handler: service.Hybrid, Method: "GET"},
	}
	service.Actions = actions

	group := router.Group("/api")

	for idx, ctrl := range service.Actions {
		log.Printf("FileController (%v): %s - %s", idx, ctrl.Pattern, ctrl.Method)

		if ctrl.Method == "GET" {
			group.GET(ctrl.Pattern, ctrl.Handler)
		} else if ctrl.Method == "POST" {
			group.POST(ctrl.Pattern, ctrl.Handler)
		}
	}

	return service
}

// @BasePath /api

// Download godoc
// @Summary download file
// @Schemes
// @Description download file
// @Success 200 {binary} FileContent
// @Router /file/download/{path} [get]
func (ctrl FileController) Download(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Download: %s", path)
	//TODO: finish him!
	content := ctrl.DocumentBlobProvider.GetContent(path)

	context.Header("ContentType", content.ContentType)
	context.Writer.Write(content.Content)
}

// Download Text godoc
// @Summary Download Text
// @Schemes
// @Description Download Text
// @Produce text/plain
// @Success 200 {binary} FileContent
// @Router /file/text/{path} [get]
func (ctrl FileController) Text(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Text: %s", path)
	//TODO: fix this
}

// Download HTML godoc
// @Summary Download HTML
// @Schemes
// @Description Download HTML
// @Produce text/html
// @Success 200 {binary} FileContent
// @Router /file/html/{path} [get]
func (ctrl FileController) Html(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Html: %s", path)
	//TODO: fix this
}

// Download PDF godoc
// @Summary Download PDF
// @Schemes
// @Description Download PDF
// @Produce text/plain
// @Success 200 {binary} FileContent
// @Router /file/pdf/{path} [get]
func (ctrl FileController) Pdf(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Pdf: %s", path)
	//TODO: fix this
}

// Download Summary godoc
// @Summary Download Summary
// @Schemes
// @Description Download Summary
// @Produce text/plain
// @Success 200 {binary} FileContent
// @Router /file/summary/{path} [get]
func (ctrl FileController) Summary(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Summary: %s", path)
	//TODO: fix this
}

// List all files godoc
// @Summary List all files
// @Schemes
// @Description List all files
// @Produce json
// @Success 200 {json} SearchModelResult[]
// @Router /file/list [get]
func (ctrl FileController) List(context *gin.Context) {
	log.Printf("List")

	result := ctrl.DocumentBlobProvider.List()
	context.JSON(200, result)
}

// Generate embedding godoc
// @Summary Generate embedding
// @Schemes
// @Description Generate embedding
// @Produce json
// @Param text query string true "text for embedding"
// @Success 200 {json} float32[]
// @Router /file/embed [get]
func (ctrl FileController) Embed(context *gin.Context) {
	log.Printf("Embed")

	text := context.Query("text")

	result, _ := ctrl.EmbeddingProvider.Embed(text)
	context.JSON(200, result)
}

// Semantic Search godoc
// @Summary Semantic Search
// @Schemes
// @Description Semantic Search
// @Produce json
// @Param query query string true "query string"
// @Param limit query int false "page length"
// @Success 200 {json} SearchResultWithSummaryModel[]
// @Router /file/semantic [get]
func (ctrl FileController) Semantic(context *gin.Context) {
	log.Printf("Semantic")

	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.SemanticSearchProvider.Search(query, limit)
	context.JSON(200, result)
}

// Lexical Search godoc
// @Summary Lexical Search
// @Schemes
// @Description Lexical Search
// @Produce json
// @Param query query string true "query string"
// @Param limit query int false "page length"
// @Success 200 {json} SearchResultWithSummaryModel[]
// @Router /file/lexical [get]
func (ctrl FileController) Lexical(context *gin.Context) {
	log.Printf("Lexical")

	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.LexicalSearchProvider.Search(query, limit)
	context.JSON(200, result)
}

// Hybrid Search godoc
// @Summary Hybrid Search
// @Schemes
// @Description Hybrid Search
// @Produce json
// @Param query query string true "query string"
// @Param limit query int false "page length"
// @Success 200 {json} SearchResultWithSummaryModel[]
// @Router /file/hybrid [get]
func (ctrl FileController) Hybrid(context *gin.Context) {
	log.Printf("Hybrid")

	// writer http.ResponseWriter, request *http.Request,
	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.HybridSearchProvider.Search(query, limit)

	context.JSON(200, result)
}
