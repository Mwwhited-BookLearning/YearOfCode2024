package providers

import (
	"hybrid-search/webapi/models"
)

type SemanticSearchProvider interface {
	Search(query string, limit int) []models.SearchResultWithSummaryModel
}
