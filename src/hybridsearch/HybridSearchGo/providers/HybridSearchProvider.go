package providers

import (
	"hybrid-search/webapi/models"
)

type HybridSearchProvider interface {
	Search(query string, limit int) []models.SearchResultWithSummaryModel
}
