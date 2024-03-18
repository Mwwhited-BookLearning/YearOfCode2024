package files

import (
	"net/http"
	"os"
	"strings"
)

// type Route struct {
// 	Pattern string
// 	Handler func(http.ResponseWriter, *http.Request)
// }

// type FileController interface {
// 	Download(writer http.ResponseWriter, request *http.Request)
// 	Text(writer http.ResponseWriter, request *http.Request)
// 	Html(writer http.ResponseWriter, request *http.Request)
// 	Pdf(writer http.ResponseWriter, request *http.Request)
// 	Summary(writer http.ResponseWriter, request *http.Request)
// 	Embed(writer http.ResponseWriter, request *http.Request)
// 	Semantic(writer http.ResponseWriter, request *http.Request)
// 	Lexical(writer http.ResponseWriter, request *http.Request)
// 	Hybrid(writer http.ResponseWriter, request *http.Request)
// }

func AddRoutes() {
	http.HandleFunc("/file/download/", Download)
	http.HandleFunc("/file/text/", Text)
	http.HandleFunc("/file/html/", Html)
	http.HandleFunc("/file/pdf/", Pdf)
	http.HandleFunc("/file/summary/", Summary)

	http.HandleFunc("/file/embed/", Embed)

	http.HandleFunc("/file/semantic/", Semantic)
	http.HandleFunc("/file/lexical/", Lexical)
	http.HandleFunc("/file/hybrid/", Hybrid)
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
	os.Stdout.WriteString("Download: " + path)
}

func Text(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/text/")
	os.Stdout.WriteString("Text: " + path)
}

func Html(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/html/")
	os.Stdout.WriteString("Html: " + path)
}

func Pdf(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/pdf/")
	os.Stdout.WriteString("Pdf: " + path)
}

func Summary(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/summary/")
	os.Stdout.WriteString("Summary: " + path)
}

func Embed(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/embed/")
	os.Stdout.WriteString("Embed: " + path)
}

func Semantic(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/semantic/")
	os.Stdout.WriteString("Semantic: " + path)
}

func Lexical(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/lexical/")
	os.Stdout.WriteString("Lexical: " + path)
}

func Hybrid(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/hybrid/")
	os.Stdout.WriteString("Hybrid: " + path)
}
