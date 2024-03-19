package files

import (
	"log"
	"net/http"
	"strings"
)

type Route struct {
	Pattern string
	Handler func(http.ResponseWriter, *http.Request)
}

type FileService interface {
	CreateRoutes() []Route
	AddRoutes(routes []Route)
}

func CreateRoutes() []Route {
	return []Route{
		{Pattern: "/file/download/", Handler: Download},
		{Pattern: "/file/text/", Handler: Text},
		{Pattern: "/file/html/", Handler: Html},
		{Pattern: "/file/pdf/", Handler: Pdf},
		{Pattern: "/file/summary/", Handler: Summary},

		{Pattern: "/file/embed/", Handler: Embed},

		{Pattern: "/file/semantic/", Handler: Semantic},
		{Pattern: "/file/lexical/", Handler: Lexical},
		{Pattern: "/file/hybrid/", Handler: Hybrid},
	}
}

func AddRoutes(routes []Route) {
	for _, ctrl := range routes {
		http.HandleFunc(ctrl.Pattern, ctrl.Handler)
	}
}

func getPath(request *http.Request, basePath string) string {
	path := request.URL.Path[len(basePath):]
	if strings.HasPrefix(path, "/") {
		path = path[1:]
	}
	return path
}

func Download(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/download/")
	log.Printf("Download: %s", path)
}

func Text(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/text/")
	log.Printf("Text: %s", path)
}

func Html(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/html/")
	log.Printf("Html: %s", path)
}

func Pdf(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/pdf/")
	log.Printf("Pdf: %s", path)
}

func Summary(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/summary/")
	log.Printf("Summary: %s", path)
}

func Embed(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/embed/")
	log.Printf("Embed: %s", path)
}

func Semantic(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/semantic/")
	log.Printf("Semantic: %s", path)
}

func Lexical(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/lexical/")
	log.Printf("Lexical: %s", path)
}

func Hybrid(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/hybrid/")
	log.Printf("Hybrid: %s", path)
}
