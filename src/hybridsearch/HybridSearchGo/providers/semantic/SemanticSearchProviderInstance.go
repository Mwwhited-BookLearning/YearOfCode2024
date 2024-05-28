package semantic

import (
	"hybrid-search/webapi/models"
)

type SemanticSearchProviderInstance struct {
}

func Create() SemanticSearchProviderInstance {
	return SemanticSearchProviderInstance{}
}

func (provider SemanticSearchProviderInstance) Search(query string, limit int) []models.SearchResultWithSummaryModel {
	return nil //TODO: complete this
}
