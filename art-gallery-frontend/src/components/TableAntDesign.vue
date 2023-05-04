<template>
    <div class="main-div">
        <div class="table-div">
            <a-table
                class="ant-table"
                :columns="columns"
                :data-source="exhibitions"
                bordered>
            </a-table>
            <a-button type="primary" size="large" class="plus-button" v-on:click="showInputs" v-if="isAdmin()">+</a-button>
        </div>

        <div class="post-message" v-if="resultReceived && addExhibitionClicked" >
            <div v-if="success">
                <h3 class="success-text">{{ this.postResult }}</h3>
            </div>
            <div v-else>
                <h3 class="error-text">{{ this.postResult }}</h3>
            </div>
        </div>

        <div class="add-exhibition-div" v-if="addExhibitionClicked">
            <h2>Add New Exhibition</h2>
            <input type="text" v-model="this.name" class="table-input" placeholder="Enter name..."/>
            <input type="text" v-model="this.description" class="table-input" placeholder="Enter description..."/>
            <input type="text" v-model="this.backgroundImageUrl" class="table-input" placeholder="Enter backgroundImageUrl..."/>
            <!-- Added to match the new model - might be better as a special date input? -->
            <input type="text" v-model="this.startDate" class="table-input" placeholder="Enter start date as DD/MM/YYYY..."/>
            <input type="text" v-model="this.endDate" class="table-input" placeholder="Enter end date as DD/MM/YYYY..."/>
            <a-button type="primary" size="large" v-on:click="addExhibition">Add exhibition</a-button>
        </div>
    </div>
</template>

<script>
import { postExhibition } from '@/services/ExhibitionService';
import { mapState } from 'vuex';

export default {
    name: "TableAntDesign",
    props: {
        exhibitions: {
            type : Array
        },
        fields: {
            type: Array
        }
    },
    data() {
        return {
            name: "",
            description: "",
            backgroundImageUrl: "",
            startDate: "",
            endDate: "",
            search: "",
            addExhibitionClicked: false,
            postResult: "",
            resultReceived: false,
            success: false,
            columns: [
                {
                    title: 'Image',
                    align: 'center',
                    dataIndex: 'backgroundImageUrl',
                    key: 'backgroundImageUrl',
                    customRender: (data) => {
                        return <img src={data.text}
                        style="max-height:200px; width:300px; object-fit:cover"/>
                    }
                },
                {
                    title: 'Name',
                    align: 'center',
                    sorter: (a, b) => a.name.localeCompare(b.name),
                    dataIndex: 'name',
                    key: 'name',
                },
                {
                    title: 'Description',
                    align: 'center',
                    sorter: (a, b) => a.description.localeCompare(b.description),
                    responsive: ["sm"],
                    dataIndex: 'description',
                    key: 'description',
                },
                {
                    title: 'Start',
                    align: 'center',
                    sorter: (a, b) => this.dateCompare(a.startDate, b.startDate),
                    filters: [
                        {
                            text: '2022',
                            value: '2022',
                        },
                        {
                            text: '2023',
                            value: '2023',
                        },
                        {
                            text: '2024',
                            value: '2024',
                        },
                    ],
                    filterMultiple: false,
                    onFilter: (a, b) => b.startDate.indexOf(a) == 0,
                    responsive: ["md"],
                    dataIndex: 'startDate',
                    key: 'startDate'
                },
                {
                    title: 'End',
                    align: 'center',
                    sorter: (a, b) => this.dateCompare(a.startDate, b.startDate),
                    filters: [
                        {
                            text: '2022',
                            value: '2022',
                        },
                        {
                            text: '2023',
                            value: '2023',
                        },
                        {
                            text: '2024',
                            value: '2024',
                        },
                    ],
                    filterMultiple: false,
                    onFilter: (a, b) => b.startDate.indexOf(a) == 0,
                    responsive: ["md"],
                    dataIndex: 'endDate',
                    key: 'endDate'
                }
            ]
        }
    },
    methods: {
        // Compares two dates returning int indicating larger, smaller or equal.
        dateCompare(date1, date2) {
            if (date1 > date2) {
                return 1;
            }
            else if (date1 < date2) {
                return -1;
            }
            else {
                return 0;
            }
        },
        isAdmin() {
            if (this.account.user) {
                return this.account.user.role == "Admin";
            }
        },
        // Show the add entry inputs.
        showInputs() {
            if (!this.addExhibitionClicked)
                this.addExhibitionClicked = true;
            else
                this.addExhibitionClicked = false;
        },
        // Add entry to database.
        async addExhibition() {
            // Check for authentication credentials - doesn't check if the user is an admin, just that there is a user
            if (!this.account.user)
            {
                console.log('Error: not logged in')
            }
            if (this.account.user)
            {
                console.log(this.name);
                this.postResult = await postExhibition(this.name, this.description, this.backgroundImageUrl, this.startDate, this.endDate);

                if (this.postResult.name == this.name) {
                    this.postResult = "Your upload has been successful."
                    this.success = true
                }
                else 
                    this.success = false
 
                this.resultReceived = true
            }

            if (this.exhibitions.name != "undefined")
            {
                this.$emit('updateData');
            }
        },
    },
    computed: {
        ...mapState({
            account: state => state.account
        })
    }
}
</script>

<style scoped>

    .main-div {
        margin-top: 50px;
        margin-bottom: 20px;
        margin-left: auto;
        margin-right: auto;
        text-align: center;
    }

    .table-div {
        margin-left: auto;
        margin-right: auto;
        width: 100%;
        overflow-x:auto;
    }

    .ant-table {
        color: var(--color--grey-dark);
        padding: 20px;
        font-family: var(--font--base);
    }

    .search-input {
        width: 50%;
        max-width: 800px;
        padding: 10px;
        border: var(--color--charcoal) solid 1px;
        margin-bottom: 20px;
        font-size: 16px;
        vertical-align: top;
    }

    .search-input::placeholder {
        font-size: 16px;
    }

    .search-icon {
        margin-right: 15px;
        display: inline;
        vertical-align: top;
    }

    .search-icon svg {
        padding-top: 5px;
        fill: var(--color--black);
        max-width: 28px;
        width: calc(24px + 6 * (100vw - 320px) / 1040);
    }

    .plus-button {
        float:right;
        margin-bottom: 20px;
    }

    .table-input {
        display: block;
        padding: 10px;
        font-size: 16px;
        margin-right: 10px;
        margin-bottom: 15px;
        border: 0.5px solid rgb(146, 146, 146);
        border-radius: 3px;
        width: 100%;
    }

    .add-exhibition-div {
        display:block;
        background-color: rgb(245, 245, 245);
        box-shadow: 5px 10px rgb(227, 226, 226);
        border-radius: 10px;
        padding:30px;
        margin-left: auto;
        margin-right:auto;
        margin-bottom: 20px;
        max-width: 500px;
        width: 100%;
    }

    .success-text {
        color:green;
    }

    .error-text {
        color:red;
    }

    .post-message {
        margin-bottom:30px;
    }


    @media only screen and (max-width: 600px) {
        .img-exh {
            padding-top: 1px;
            padding-bottom: 1px;
            padding-left: 1px;
            padding-right: 1px;
            max-height: 120px;
            max-width: 100px;
        }
    }

</style>