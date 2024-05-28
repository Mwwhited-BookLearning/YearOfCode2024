package providers

import (
	"hybrid-search/webapi/models"
)

type LexicalSearchProvider interface {
	Search(query string, limit int) []models.SearchResultWithSummaryModel
}
