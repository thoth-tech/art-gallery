<template>
    <div class="main-div">
        <!-- ... Existing code ... -->

        <button @click="toggleAddExhibition" class="plus-button" v-if="isAdmin()">+</button>

        <!-- ... Existing code ... -->

        <div class="entry-div" v-if="addExhibitionClicked">
            <h2>Add New Exhibition</h2>
            <!-- ... Existing code ... -->
            <button @click="addExhibition" class="add-exhibition-button">Add exhibition</button>
            <!-- Add a cancel button to hide the entry form -->
            <button @click="cancelAddExhibition" class="add-exhibition-button">Cancel</button>
        </div>
    </div>
</template>

<script>
// ... Existing imports ...

export default {
    // ... Existing code ...

    methods: {
        // ... Existing methods ...

        // Toggle the addExhibitionClicked state
        toggleAddExhibition() {
            this.addExhibitionClicked = !this.addExhibitionClicked;
            // Clear the input fields when toggling
            if (!this.addExhibitionClicked) {
                this.clearInputFields();
            }
        },

        // Clear input fields
        clearInputFields() {
            this.name = "";
            this.description = "";
            this.backgroundImageUrl = "";
            this.startDate = "";
            this.endDate = "";
            this.resultReceived = false;
            this.success = false;
        },

        // Cancel adding exhibition and clear input fields
        cancelAddExhibition() {
            this.toggleAddExhibition();
        },

        // ... Existing methods ...
    },
};
</script>

<style scoped>
/* ... Existing styles ... */
</style>